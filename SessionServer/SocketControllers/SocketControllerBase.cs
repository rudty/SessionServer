namespace Sessions.SocketControllers {
    public class SocketControllerBase {

        protected SocketControllerBase() {
            SocketControllerDispatcher.Instance.Register(this);
        }
    }
}
