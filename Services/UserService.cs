using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using alumni.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Alumni.Helpers;
using Microsoft.AspNetCore.Identity;
using alumni.Contracts.V1.Responses;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace alumni.Services
{
    public class UserService : IUserService
    {
        private DataContext dataContext { get; set; }

        private readonly UserManager<User> userManager;

        private readonly Options.TokenOptions tokenOptions;

        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(UserManager<User> userManager,
            Options.TokenOptions tokenOptions,
            RoleManager<IdentityRole> roleManager,
            DataContext dataContext)
        {
            this.userManager = userManager;

            this.roleManager = roleManager;

            this.tokenOptions = tokenOptions;

            this.dataContext = dataContext;
        }


        #region(Authentication)
        public async Task<AuthResult> LoginAsync(LoginDomain login)
        {
            var user = await userManager.FindByEmailAsync(login.Email);

            if (user == null) return new AuthResult
            {
                Authenticated = false,
                Errors = new[] { Constants.ServerMessages.LoginAuthFail }
            };

            var credentialsValid = await userManager.CheckPasswordAsync(user, login.Password);

            if (!credentialsValid) return new AuthResult
            {
                Authenticated = false,
                Errors = new[] { Constants.ServerMessages.LoginAuthFail }
            };

            //var userLoginInfo = new UserLoginInfo(login.LoginProvider, login.LoginKey, login.LoginProvider);

            //await userManager.AddLoginAsync(user, userLoginInfo);

            return await CreateTokenAsync(user);
        }

        public async Task<AuthResult> RegistrationAsync(User user, AuthData auth)
        {
            if (user == null) return null;

            var userExists = await userManager.FindByEmailAsync(user.Email) != null;

            if (userExists)
                return new AuthResult { Authenticated = false, Errors = new[] { Constants.ServerMessages.EmailAlreadyExists } };

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Situation = Constants.SituationsObjects.NormalSituation,
                EmailConfirmed = true,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.Email,
                DateSituation = DateTime.Now,
                Picture = user.Picture
            };

            var stt = await userManager.CreateAsync(newUser);

            if (!stt.Succeeded)
                return new AuthResult { Errors = stt.Errors.Select(e => e.Description) };

            var sttA = await userManager.AddPasswordAsync(newUser, auth.Password);
            if (!sttA.Succeeded) return AuthFail();

            var sttB = await userManager.AddToRoleAsync(newUser, auth.Role);
            if (!sttB.Succeeded) return AuthFail();

            if (auth.Role.Equals(Constants.UserContansts.SchoolRole))
                return new AuthResult
                {   Authenticated= true,
                    User = newUser
                };

            return await CreateTokenAsync(newUser);
        }

        public async Task<string> GetRoleAsync(string id)
        {
            var user = await GetUserAsync(id);

            return (await userManager.GetRolesAsync(user))[0];
        }

        public async Task<AuthResult> RefreshTokenAsync(string token, string refreshToken)
        {
            if (token == null) return null;

            try
            {
                var securityToken = ValidateToken(token);

                if (securityToken == null)
                    return AuthFail();

                var succeded = IsJwtSecurityToken(securityToken);

                if (!succeded)
                    return AuthFail();

                var isValid = await ValidateRefreshToken(securityToken as JwtSecurityToken, refreshToken);

                if (!isValid)
                    return AuthFail();

                var user = await GetUserFromToken(securityToken as JwtSecurityToken);

                if (user == null) return AuthFail();

                return await CreateTokenAsync(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return AuthFail();
            }
        }

        public async Task<AuthResult> CreateTokenAsync(User user)
        {
            if (user != null)
            {
                var tokenId = Guid.NewGuid().ToString();

                var tokenHandler = new JwtSecurityTokenHandler();

                var subject = await GenerateTokenClaims(user, tokenId);

                var descriptor = new SecurityTokenDescriptor
                {
                    Audience = tokenOptions.Audience,
                    SigningCredentials = tokenOptions.SigningCredentials,
                    Expires = DateTime.UtcNow.AddHours(tokenOptions.LifeTime),
                    NotBefore = DateTime.UtcNow,
                    Subject = subject
                };

                var securityJwtToken = tokenHandler.CreateToken(descriptor);

                if (tokenHandler.CanWriteToken)
                {
                    var token = tokenHandler.WriteToken(securityJwtToken);

                    if (token != null)
                    {
                        var authConfig = await GenerateTokenConfig(user, tokenId, token);

                        if (authConfig == null) return AuthFail();

                        return new AuthResult
                        {
                            Authenticated = true,
                            User = user,
                            AuthConfigTokens = authConfig,
                        };
                    }
                }
            }

            return AuthFail();
        }

        public async Task<User> GetUserAsync(string id)
        {
            if (id == null) return null;

            var user = await userManager.FindByIdAsync(id);

            if (user == null) return null;

            return user;
        }

        public async Task<bool> UpdateAsync(string id, User user)
        {
            if (user.Id != id) return false;

            dataContext.Entry(user).State = EntityState.Modified;
            try
            {
                await dataContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(PaginationFilter filter = null)
        {
            var users = userManager.Users;

            if (filter == null) return await users.ToListAsync();

            if (filter.SearchValue != null)
            {
                var sv = filter.SearchValue;

                /*users = users.Where(u => u.FirstName.Contains(sv) || u.LastName.Contains(sv));*/
            }

            var skip = (filter.PageNumber - 1) * filter.PageSize;

            users = users.Skip(skip).Take(filter.PageSize);

            return await users.ToListAsync();
        }

        public async Task<Teacher> GetTeacherAsync(string userId)
        {
            return await dataContext.Teachers
                    .Include(t => t.AcademicLevel)
                    .Include(t => t.Academy)
                    .Include(t => t.Course)
                    .Include(t => t.User)
                    .SingleOrDefaultAsync(t => t.User.Id == userId && t.User.Situation == Constants.SituationsObjects.NormalSituation);
        }

        public async Task<Studant> GetStudantAsync(string userId)
        {
            return await dataContext.Studants
                           /*.Include(s => s.AcademicLevel)*/
                           /*.Include(s => s.Academy)*/
                           /*.Include(s => s.Course)*/
                           .Include(s => s.User)
                           .SingleOrDefaultAsync(s => s.User.Situation == Constants.SituationsObjects.NormalSituation && s.UserId == userId);
        }

        private async Task<User> GetUserFromToken(JwtSecurityToken token)
        {
            if (token == null) return null;

            var userId = token.Claims.SingleOrDefault(c => c.Type == Constants.UserContansts.IdClaimType).Value;

            return await userManager.FindByIdAsync(userId);
        }

        private SecurityToken ValidateToken(string token)
        {
            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParams = new TokenValidationParameters
                {
                    IssuerSigningKey = tokenOptions.Key,
                    ValidAudience = tokenOptions.Audience,
                    ValidIssuer = tokenOptions.Issuer,
                    ClockSkew = tokenOptions.ClockSkew,
                    ValidateLifetime = false,
                    ValidateAudience = tokenOptions.ValidateAudience,
                    ValidateIssuer = tokenOptions.ValidateIssuer
                };

                try
                {
                    tokenHandler.ValidateToken(token, validationParams, out var securityToken);

                    return securityToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                    return null;
                }
            }

            return null;
        }

        private bool IsJwtSecurityToken(SecurityToken securityToken)
        {
            if (securityToken != null)
            {
                if (securityToken is JwtSecurityToken)
                {
                    var jwtSecurityToken = securityToken as JwtSecurityToken;

                    return jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256);
                }
            }

            return false;
        }

        private async Task<ClaimsIdentity> GenerateTokenClaims(User user, string tokenId)
        {
            var subject = new ClaimsIdentity(
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Email,user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                        new Claim(Constants.UserContansts.IdClaimType, user.Id),
                        new Claim(JwtRegisteredClaimNames.Iss, "www.alumni.com"),
                        new Claim(JwtRegisteredClaimNames.Aud, "www.alumni.com")
                    });

            var claims = await userManager.GetClaimsAsync(user);
            subject.AddClaims(claims);

            var roleNames = await userManager.GetRolesAsync(user);

            foreach (var roleName in roleNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);

                if (role == null) return null;

                subject.AddClaim(new Claim(ClaimTypes.Role, roleName));

                var roleClaims = await roleManager.GetClaimsAsync(role);

                if (roleClaims == null) continue;

                foreach (var claim in claims)
                {
                    if (!subject.Claims.Contains(claim))
                        subject.AddClaim(claim);
                    continue;
                }

            }

            return subject;
        }

        private async Task<bool> ValidateRefreshToken(JwtSecurityToken token, string refreshToken)
        {
            if (token == null || refreshToken == null) return false;

            var jti = token.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var refreshTokenStorage = await dataContext.AuthConfigTokens
                                            .SingleOrDefaultAsync(rt => rt.RefreshToken == refreshToken && rt.Jwti == jti);

            if (refreshTokenStorage == null) return false;

            if (refreshTokenStorage.Invalidated) return false;

            if (refreshTokenStorage.Used && refreshTokenStorage.ExpireAt < DateTime.UtcNow) return false;

            if (refreshTokenStorage.ExpireAt < DateTime.UtcNow)
            {
                refreshTokenStorage.Invalidated = true;
                return false;
            }

            var expiryDateUnix = long.Parse(token.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            var creationDateUnix = long.Parse(token.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Nbf).Value);

            var creationDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(creationDateUnix);

            if (refreshTokenStorage.ExpireAt < DateTime.UtcNow && expiryDateTimeUtc < DateTime.UtcNow)
                return false;

            // Desable to test
            //if (expiryDateTimeUtc > DateTime.UtcNow)
            //    return false;

            refreshTokenStorage.Used = true;

            return true;
        }

        private AuthResult AuthFail()
        {
            return new AuthResult { Errors = new[] { Constants.ServerMessages.TokenNotCreated } };
        }

        private async Task<AuthConfigTokens> GenerateTokenConfig(User user, string tokenId, string token)
        {
            var authConfigToken = new AuthConfigTokens
            {
                RefreshToken = Guid.NewGuid().ToString(),
                Jwti = tokenId,
                TokenValue = token,
                CreationAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddHours(tokenOptions.LifeTime),
                Invalidated = false,
                Used = false,
                UserId = user.Id
            };

            await dataContext.AddAsync(authConfigToken);

            try
            {
                var stt = await dataContext.SaveChangesAsync();

                if (stt > 0)
                {
                    return authConfigToken;
                }
            }
            catch (DbUpdateException)
            {
                return null;
            }


            return null;
        }


        #endregion
    }
}
