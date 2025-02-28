using System.Threading;

namespace SimpleTool.Request
{
	//	A list of requests the SplitWallSettings dialog has available
	public enum SimpleToolRequestId : int
	{
		None = 0,

		SettingForm,
	}

	//	A class around a variable holding the current request.
	//	<remarks>
	//		Access to it is made thread-safe, even though we don't necessarily
	//		need it if we always disable the dialog between individual requests.
	//	</remarks>
	public class SimpleToolRequest
	{
		// Storing the value as a plain Int makes using the interlocking mechanism simpler
		private int m_request = (int)SimpleToolRequestId.None;

		//  Take - The Idling handler calls this to obtain the latest request. 
		//  <remarks>
		//      This is not a getter! It takes the request and replaces it
		//      with 'None' to indicate that the request has been "passed on".
		//  </remarks>
		public SimpleToolRequestId Take()
		{
			return (SimpleToolRequestId)Interlocked.Exchange(ref m_request, (int)SimpleToolRequestId.None);
		}

		//  Make - The Dialog calls this when the user presses a command button there. 
		//  <remarks>
		//      It replaces any older request previously made.
		//  </remarks>
		public void Make(SimpleToolRequestId request)
		{
			Interlocked.Exchange(ref m_request, (int)request);
		}
	}
}
