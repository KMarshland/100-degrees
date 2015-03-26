using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class LogControl {

	static readonly bool disableWhenNotPlaying = true;

	static List<LogItem> allLogs = new List<LogItem>();

	static Dictionary<LogType, List<LogItem>> logs = new Dictionary<LogType, List<LogItem>>(){
		{LogType.Log, new List<LogItem>()},
		{LogType.Warning, new List<LogItem>()},
		{LogType.Error, new List<LogItem>()}
	};

	private static void log(object message, Object context, LogType logType){
		//create a log item
		LogItem li;
		if (context == null){
			li = new LogItem(message, logType);
		} else {
			li = new LogItem(message, context, logType);
		}

		//remember that this existed
		logs[logType].Add(li);
		allLogs.Add(li);

		//actually print it
		if (!disableWhenNotPlaying || Application.isPlaying){
			li.LogWithType();
		}
	}
	public static void Log(object message){
		Log(message, null);
	}
	public static void LogWarning(object message){
		Log(message, null);
	}
	public static void LogError(object message){
		Log(message, null);
	}
	public static void Log(object message, Object context){
		log(message, context, LogType.Log);
	}
	public static void LogWarning(object message, Object context){
		log(message, context, LogType.Warning);
	}
	public static void LogError(object message, Object context){
		log(message, context, LogType.Error);
	}

	class LogItem {
		object message;
		Object context;

		LogType logType;

		public LogItem(object nmessage, LogType nlogType){
			message = nmessage;
			context = null;
			logType = nlogType;
		}

		public LogItem(object nmessage, Object ncontext, LogType nlogType){
			message = nmessage;
			context = ncontext;
			logType = nlogType;
		}

		public void LogWithType(){
			if (logType == LogType.Log){
				this.Log();
			} else if (logType == LogType.Warning){
				this.LogWarning();
			} else if (logType == LogType.Error){
				this.LogError();
			}
		}

		public void Log(){
			if (context != null){
				Debug.Log(message, context);
			} else {
				Debug.Log(message);
			}
		}

		public void LogWarning(){
			if (context != null){
				Debug.LogWarning(message, context);
			} else {
				Debug.LogWarning(message);
			}
		}

		public void LogError(){
			if (context != null){
				Debug.LogError(message, context);
			} else {
				Debug.LogError(message);
			}
		}
	}

}
