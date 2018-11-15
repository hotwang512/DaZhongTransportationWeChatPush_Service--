using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_WeChatPushDetail_Information{
                        
    public string Type {get;set;}

    public string PushObject {get;set;}

    public DateTime CreatedDate {get;set;}

    public string CreatedUser {get;set;}

    public DateTime ChangeDate {get;set;}

    public string ChangeUser {get;set;}

    public Guid Vguid {get;set;}

    public Guid Business_WeChatPushVguid {get;set;}

    public string ISRead { get; set; }

   }
            
}