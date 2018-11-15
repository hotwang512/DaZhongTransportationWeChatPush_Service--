using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_ExercisesAnswer_Information
    {

        public int TotalScore { get; set; }

        public int Marking { get; set; }

        public int Status { get; set; }

        public string PicturePath { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid Vguid { get; set; }

        public Guid BusinessPersonnelVguid { get; set; }

        public Guid BusinessExercisesVguid { get; set; }

    }

}