using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Sys_User{
                        
    public string LoginName {get;set;}

    public string UserName {get;set;}

    public string Password {get;set;}

    public string Company {get;set;}

    public string Email {get;set;}

    public string WorkPhone {get;set;}

    public string MobileNnumber {get;set;}

    public string Enable {get;set;}

    public string Role {get;set;}

    public string Department {get;set;}

    public DateTime CreatedDate {get;set;}

    public string CreatedUser {get;set;}

    public DateTime ChangeDate {get;set;}

    public string ChangeUser {get;set;}

    public Guid Vguid {get;set;}

   }
            
}