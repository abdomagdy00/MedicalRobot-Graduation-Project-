
namespace Application.Services
{
    public class FaceEnrollmentSessionManager
    {
        private TaskCompletionSource<int>? _tcs;

        public void StartSession()
        {
            _tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        public async Task<int> WaitForFaceIdAsync(int timeoutSeconds)
        {
            if (_tcs == null)
                throw new InvalidOperationException("No active enrollment session.");

            var delayTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
            var completedTask = await Task.WhenAny(_tcs.Task, delayTask);

            if (completedTask == delayTask)
            {
                _tcs = null;
                throw new TimeoutException("Face learning timed out. The robot did not respond.");
            }

            int faceId = await _tcs.Task;
            _tcs = null;
            return faceId;
        }

        public bool CompleteSession(int faceId)
        {
            if (_tcs != null && !_tcs.Task.IsCompleted)
            {
                _tcs.SetResult(faceId);
                return true;
            }
            return false;
        }
    }
}
