using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_ExercisesAnswerDetail_Information
    {

        public string Answer { get; set; }

        public int Score { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid Vguid { get; set; }

        public Guid BusinessExercisesDetailVguid { get; set; }

        public Guid BusinessAnswerExercisesVguid { get; set; }

    }
}