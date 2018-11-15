using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Sys_Module{
                        
    public string ModuleName {get;set;}

    public string Parent {get;set;}

    public DateTime CreatedDate {get;set;}

    public string CreatedUser {get;set;}

    public DateTime ChangeDate {get;set;}

    public string ChangeUser {get;set;}

    public Guid Vguid {get;set;}

   }
            
}