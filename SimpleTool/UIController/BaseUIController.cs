
namespace SimpleTool.UIController
{
	public class BaseUIController
	{
		#region IExternal implementation

		public virtual int GetRequestId()
		{
			return 0;
		}

		public virtual void MakeRequest(int request)
		{

		}

		public virtual void WakeUp(bool bFinish = false)
		{

		}

		#endregion

		#region Controller implementation

		public virtual void OnOK()
		{

		}

		public void OnCancel()
		{

		}

		#endregion
	}
}
