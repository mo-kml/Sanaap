using Sanaap.App.Helpers;

namespace Sanaap.App.Droid.Helpers
{
    public class AndroidAppUtilities : IAppUtilities
    {
        public void Exit()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}
