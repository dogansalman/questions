using System.Collections.Generic;

namespace QuestionsSYS.ModelViews
{
    public class PersonnelOrderState
    {
        private string state_;

        public string state
        {
            get { return state_; }
            set { state_ = value; }
        }
        private int count_;

        public int count
        {
            get { return count_; }
            set { count_ = value; }
        }

    }


    public class PersonnelReports
    {
        private string fullname_;

        public string fullname
        {
            get { return fullname_; }
            set { fullname_ = value; }
        }
        private List<PersonnelOrderState> states_;

        public List<PersonnelOrderState> states
        {
            get { return states_; }
            set { states_ = value; }
        }
        private int tasks_;

        public int tasks
        {
            get { return tasks_; }
            set { tasks_ = value; }
        }


    }
}