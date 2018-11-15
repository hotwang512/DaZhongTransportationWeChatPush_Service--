using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Master_Organization{
                        
    public string OrganizationName {get;set;}

    public string Description {get;set;}

    public Guid ParentVguid {get;set;}

    public DateTime CreatedDate {get;set;}

    public string CreatedUser {get;set;}

    public DateTime ChangeDate {get;set;}

    public string ChangeUser {get;set;}

    public Guid Vguid {get;set;}

   }
            
}