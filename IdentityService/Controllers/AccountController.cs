using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Music.DataAccess.Services;
using Music.Shared.Dtos;
using Music.Shared.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Controllers
{
    [Route("api/identity/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,IFileService fileService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _fileService = fileService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
               
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { Status = "Error", Message = "Registration failed!", Errors = result.Errors });
            }

       
            return Ok(new { Status = "Success", Message = "User registered successfully!" });
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginModel dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid email or password");

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpGet("profile")]
        [Authorize]

        public async Task<IActionResult> GetProfile()

        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)

            {

                return NotFound("User Not Found");

            }

            var dto = new ProfileDto

            {

                FullName = user.FullName,

                Email = user.Email,

                ProfileImageUrl = user.ProfileImageUrl,

                CreatedAt = user.CreatedAt,

            };

            return Ok(dto);

        }
        [HttpPost("upload-profile-image")]
        [Authorize]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            // FileService vasitəsilə faylı yükləyirik
            var relativeFilePath = await _fileService.UploadFileAsync(file);
            if (string.IsNullOrEmpty(relativeFilePath))
                return BadRequest("File upload failed");

            // İstəsən, istifadəçinin profil şəkil URL-sini əldə etmək üçün file URL metodundan istifadə et:
            user.ProfileImageUrl = relativeFilePath; // Ya da: _fileService.GetFileUrl(relativeFilePath)
            await _userManager.UpdateAsync(user);

            return Ok(new { ImageUrl = user.ProfileImageUrl });
        }

        //[HttpPost("upload-profile-image")]

        //public async Task<IActionResult> UploadProfileImage(IFormFile file)

        //{

        //    if (file == null || file.Length == 0)

        //        return BadRequest("File is empty");

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var user = await _userManager.FindByIdAsync(userId);

        //    if (user == null)

        //        return NotFound("User not found");

        //    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile-images");

        //    if (!Directory.Exists(uploadsFolder))

        //        Directory.CreateDirectory(uploadsFolder);

        //    var fileName = $"{Guid.NewGuid()}_{file.FileName}";

        //    var filePath = Path.Combine(uploadsFolder, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))

        //    {

        //        await file.CopyToAsync(stream);

        //    }

        //    user.ProfileImageUrl = $"/profile-images/{fileName}";

        //    await _userManager.UpdateAsync(user);

        //    return Ok(new { ImageUrl = user.ProfileImageUrl });

        //}


    }
}
