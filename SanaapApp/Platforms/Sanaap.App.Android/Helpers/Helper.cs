using Sanaap.App.Helpers;

namespace Sanaap.App.Droid.Helpers
{
    public class Utility : IUtility
    {
        public void Exit()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}
