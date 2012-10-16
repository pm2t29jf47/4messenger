using System; 
using System.Collections.Generic; 
using System.Collections.Specialized; 
using System.Web.Security; 

namespace Wrox.ProCSharp.Security 
{ 
public class SampleMembershipProvider: MembershipProvider 
{ 
private Dictionary<string, string> users = new Dictionary<string, string>(); 
internal static string ManagerUserName = "Manager".ToLowerlnvariant (); 
internal static string EmployeeUserName = "Employee".ToLowerlnvariant(); 
public override void Initialize(string name, NameValueCollection config) 
{ 
users = new Dictionary<string, string>(); 
users.Add(ManagerUserName, "secret@Pa$$wOrd"); 
users.Add(EmployeeUserName, "sOme@Secret"); 
base.Initialize(name, config); 
} 
public override string ApplicationName 
{ 
get 
{ 
throw new NotlmplementedException (); 
} 
set 
{ 
throw new NotlmplementedException(); 

} 

// переопределение абстрактных членов класса Membership 
// ... 
public override bool ValidateUser(string username, string password) 
{ 
if (users.ContainsKey(username.ToLowerlnvariant())) 
{ 
return password.Equals(users[username.ToLowerlnvariant ()]); 
} 
return false; 
} 
} 
} 
