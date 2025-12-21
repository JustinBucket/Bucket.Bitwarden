<img alt="gitleaks badge" src="https://img.shields.io/badge/protected%20by-gitleaks-blue">

# Bucket.Bitwarden
Bucket.Bitwarden provides a wrapper for the Bitwarden CLI, exposing its methods for consumption programmatically.

## Installation
Use the dotnet add package command to isntall Bucket.Bitwarden
```bash
dotnet add Bucket.Bitwarden
```

## Usage
### CLI presence
The package does not contain the bitwarden CLI itself. 
Make sure you include a copy in the project root, or have it configured in your PATH so that it can be called globally. 
Alternatively, clone the repo and build it yourself, containing the CLI.
### Example
Create an instance of BitwardenCaller:
```csharp
var caller = new BitwardenCaller();
```
Create a LoginData object containing your credentials
```csharp
var loginData = new LoginData(user, password);
```
Call the login method to authenticate and sync the database
```csharp
caller.Login(loginData);
```
Generate parameters
```csharp
var params = new GetParameters(vaultItemName);
```

Retrieve vaultItems
```csharp
var vaultItems = caller.RetrieveVaultItems(params);
```

Note: RetrieveVaultItems() will pull multiple vault entries if the CLI returns multiple IDs
## License

[MIT](https://choosealicense.com/licenses/mit/)
