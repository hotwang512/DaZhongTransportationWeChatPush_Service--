using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Sys_Role{
                        
    public string Role {get;set;}

    public string Description {get;set;}

    public DateTime CreatedDate {get;set;}

    public string CreatedUser {get;set;}

    public DateTime ChangeDate {get;set;}

    public string ChangeUser {get;set;}

    public Guid Vguid {get;set;}

   }
            
}