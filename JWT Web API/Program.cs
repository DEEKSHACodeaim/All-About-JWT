using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//adding the authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = false,// can be set to true if same time span for every user is required
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});
//adding the authorization service
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseHttpsRedirection();//for safer routing through https

//setting up the GET endpoint
app.MapGet("/hello", () => "Hello, World!").RequireAuthorization();// end point is defined at the path /hello
//setting up the POST endpoint
app.MapPost("/security/createToken",
[AllowAnonymous] (User user) =>
{
    string username = builder.Configuration["User:Username"];
    string password = builder.Configuration["User:Password"];
    if (user.UserName == username && user.Password == password)
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
        var token_descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
        };

        var token_handler = new JwtSecurityTokenHandler();
        var token = token_handler.CreateToken(token_descriptor);
        var jwt_token = token_handler.WriteToken(token);
        var string_token = jwt_token;
        return Results.Ok(string_token);
    }
    return Results.Unauthorized();
});
app.UseAuthentication();
app.UseAuthorization();
app.Run();






