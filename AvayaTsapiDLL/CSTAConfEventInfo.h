
using namespace System;

namespace AvayaTsapiDLL
{

	public ref class CSTAConfEventInfo
	{
		public:
			long CallID;
			String^ CallingDevice;
			String^ CalledDevice;
			String^ UCID;
			String^ EventType;

	};
}