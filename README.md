# OperationTypeAspNetCoreAuthorization

ASP.NET Core supports [Role-based authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles) by using AuthorizeAttribute: 

``` csharp

[Authorize(Roles = "HRManager,Finance")]
public class SalaryController : Controller
{
    public IActionResult Payslip() =>
                    Content("HRManager || Finance");
}

```

Roles are hardcoded and can't be changed at runtime. `OperationTypeAttribute` derives from `AuthorizeAttribute`, it specifies an operation type for actions and allows to configure authorization dynamically:

``` csharp

public class SalaryController : Controller
{
    [OperationType("View Payslip")]
    public IActionResult Payslip() =>
                    Content("HRManager || Finance");
}

```

`OperationTypeAttribute` checks if there is an `AllowedOperationType` claim with the value of `View Payslip` in jwt token to determine whether the user can access the action.

The authorization data can be saved in database: 

``` csharp

foreach (var item in allowed-operation-types ?? new string[0])
{
    await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(ClaimTypes.AllowedOperationType, item));
}


```

When the user is logging in, add claims loaded from database into the jwt token:

``` csharp

[HttpPost("login")]
public async Task<IActionResult> Login(LoginArgs model)
{
    // ...
    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
    if (result.Succeeded)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id, ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, user.UserName),
            };

        // add user claims loaded from database
        claims.AddRange(await _userManager.GetClaimsAsync(user)); 

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));

            ApplicationRole appRole = await _roleManager.FindByNameAsync(role);
            
            // adds role claims loaded from database
            claims.AddRange(await _roleManager.GetClaimsAsync(appRole));
        }

        var jwt = GenerateToken(claims);


        return Ok(new
        {
            status = "ok",
            token = jwt,
            tokenExpiry = 60,
            refreshToken = user.RefreshToken,
            type = "Bearer",
            userName = user.UserName,
            currentAuthority = roles
        });
    }

    return Ok(new
    {
        status = "error",
        message = "..."
    });

}

```


