using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_ExercisesDetail_Infomation
    {

        public int ExerciseType { get; set; }

        public string ExerciseName { get; set; }

        public int? ExericseTitleID { get; set; }

        public string ExerciseOption { get; set; }

        public string Answer { get; set; }

        public int Score { get; set; }

        public int InputType { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid Vguid { get; set; }

        public Guid ExercisesInformationVguid { get; set; }

    }

}