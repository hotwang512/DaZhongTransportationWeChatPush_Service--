using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Sys_Role_Module{
                        
    public Boolean Read {get;set;}

    public Boolean Edit {get;set;}

    public Boolean Delete {get;set;}

    public Boolean Submit {get;set;}

    public Boolean Approved {get;set;}

    public Boolean Import {get;set;}

    public Boolean Export {get;set;}

    public Boolean Creator {get;set;}

    public DateTime CreatedDate {get;set;}

    public string CreatedUser {get;set;}

    public DateTime ChangeDate {get;set;}

    public string ChangeUser {get;set;}

    public Guid Vguid {get;set;}

    public Guid ModuleVGUID {get;set;}

    public Guid RoleVGUID {get;set;}

   }
            
}