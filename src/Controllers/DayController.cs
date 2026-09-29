using idealDay.Models;

namespace idealDay.Controllers
{
    public sealed class DayController
    {
        public IdealDayModel Model { get; }

        public DayController(IdealDayModel model)
        {
            Model = model;
        }

        public bool TryApplyInput(
            TimeSpan wakeUpTime,
            double sleepHours,
            double workHours,
            out string errorMessage)
        {
            try
            {
                Model.Update(wakeUpTime, sleepHours, workHours);
                errorMessage = string.Empty;
                return true;
            }
            catch (ArgumentException exception)
            {
                errorMessage = exception.Message;
                return false;
            }
        }

        public void Reset()
        {
            Model.Reset();
        }
    }
}
