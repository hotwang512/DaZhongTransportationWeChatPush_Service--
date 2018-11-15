using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_OperationLog{
                        
    public string EventType {get;set;}

    public string Page {get;set;}

    public string Message {get;set;}

    public string Data {get;set;}

    public string User {get;set;}

    public DateTime CreatedDate {get;set;}

    public string CreatedUser {get;set;}

    public DateTime ChangeDate {get;set;}

    public string ChangeUser {get;set;}

    public Guid Vguid {get;set;}

   }
            
}