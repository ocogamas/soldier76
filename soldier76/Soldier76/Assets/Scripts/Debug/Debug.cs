using UnityEngine;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

static public class Debug
{

    static public int LOG_ONLY_VIEW_FONT_SIZE = -1;

	private const int LOG_DEFAULT       = 1;      // Unityのデフォルト
    private const int LOG_WHITE         = 1;      // 白
    private const int LOG_CYAN          = 1;      // シアン
    private const int LOG_YELLOW        = 1;      // 黄色
    private const int LOG_MAGENTA       = 1;      // マゼンタ
    private const int LOG_LIME          = 1;      // ライム
    private const int LOG_RED           = 1;      // 赤
    private const int LOG_BLUE          = 1;      // 青
    private const int LOG_ORANGE        = 1;      // オレンジ
    private const int LOG_DEEPSKYBLUE   = 1;      // ディープスカイブルー
    private const int LOG_LIGHT_CYAN    = 1;      // ライトシアン
    private const int LOG_LIGHT_YELLOW  = 1;      // ライトイエロー
    private const int LOG_LIGHT_MAGENTA = 1;      // ライトマゼンタ
    private const int LOG_LIGHT_LIME    = 1;      // ライトライム
    private const int LOG_LIGHT_RED     = 1;      // ライトレッド
    private const int LOG_LIGHT_BLUE    = 1;      // ライトブルー
    private const int LOG_LIGHT_ORANGE  = 1;      // ライトオレンジ
    private const int LOG_LIGHT_SKYBLUE = 1;      // ライトスカイブルー




    public static void Log(object message, Object context = null)
    {

		if (LOG_DEFAULT > 0) 
		{
            if (LOG_ONLY_VIEW_FONT_SIZE < 0)
            {
                UnityEngine.Debug.Log (message, context);
            }
		}
    }

    public static void MyLog(object message, Object context = null)
	{
		UnityEngine.Debug.Log (message, context);
	}

    public static void LogWarning(object message, Object context = null)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }

    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format, args);
    }


    public static void LogException(System.Exception exception, Object context = null)
    {
        UnityEngine.Debug.LogException (exception, context);
    }

    public static void LogError(System.Exception ex, Object context = null)
    {
        string message = "Source : " + ex.Source + "\n Message : " + ex.Message + "\n StackTrace : " + ex.StackTrace;
        LogError(message, context);
    }

    public static void LogError(object message, Object context = null)
    {
        // コンソールに出力
        UnityEngine.Debug.LogError(message, context);

    }


    static public void PrintLog(string color, string message, object obj, int size)
    {
            
        // フォントサイズでのログ出力フィルタ
        // フィルタが指定されていて（>=0）
        // 指定したフォントサイズでなければreturn;
        if (LOG_ONLY_VIEW_FONT_SIZE >= 0 && LOG_ONLY_VIEW_FONT_SIZE != size)
        {
            return;
        }
            
        
        int defaultSize = 11;
        int fontSize    = size + defaultSize;

        System.DateTime dateTime = System.DateTime.Now;

        string text;
        if (obj != null) {
            string className  = obj.GetType ().Name;
            text = "" + dateTime.Second + "." + dateTime.Millisecond + " <b>" + className + "</b>  " + message;
        } else {
            text = "" + dateTime.Second + "." + dateTime.Millisecond + " " + message;
        }

		MyLog ("<color=" + color + "><size=" + fontSize + ">" + text + "</size></color>");


    }


    static public void Assert( bool f )
    {
        if( !f ) throw new System.Exception();
    }

    static public void Assert( bool f, string s )
    {
        if( !f ) throw new System.Exception( s );
    }

    /*
     *    UnityEngine.Debug wrapper
     */
    static public void Break()
    {
        UnityEngine.Debug.LogError( "Break" );
        UnityEngine.Debug.Break();
    }

    static public void DrawLine( Vector3 start, Vector3 end, Color color, float duration = 0.0f, bool depthTest = true )
    {
        UnityEngine.Debug.DrawLine( start, end, color, duration, depthTest );
    }

    static public void DrawLine( Vector3 start, Vector3 dir )
    {
        UnityEngine.Debug.DrawRay (start, dir, Color.white);
    }

    static public void DrawRay( Vector3 start, Vector3 dir, Color color, float duration = 0.0f, bool depthTest = true )
    {
        UnityEngine.Debug.DrawRay( start, dir, color, duration, depthTest );
    }

    // 
    // 色付きデバッグログ
    //
    static public void Log_white(string message, object obj=null, int size=0)       { if (LOG_WHITE > 0) PrintLog("#ffffff", message, obj, size); }

    static public void Log_cyan(string message, object obj=null, int size=0)        { if (LOG_CYAN > 0) PrintLog("#00ffff", message, obj, size); }

    static public void Log_yellow(string message, object obj=null, int size=0)      { if (LOG_YELLOW > 0) PrintLog("#ffff00", message, obj, size); }    

    static public void Log_magenta(string message, object obj=null, int size=0)     { if (LOG_MAGENTA > 0) PrintLog("#ff00ff", message, obj, size); }  

    static public void Log_lime(string message, object obj=null, int size=0)        { if (LOG_LIME > 0) PrintLog("#00ff00", message, obj, size); }

    static public void Log_red(string message, object obj=null, int size=0)         { if (LOG_RED > 0) PrintLog("#ff0000", message, obj, size); }

    static public void Log_blue(string message, object obj=null, int size=0)        { if (LOG_BLUE > 0) PrintLog("#2244ff", message, obj, size); }

    static public void Log_orange(string message, object obj=null, int size=0)      { if (LOG_ORANGE > 0) PrintLog("#ffa500", message, obj, size); }    

    static public void Log_deepskyblue(string message, object obj=null, int size=0) { if (LOG_DEEPSKYBLUE > 0) PrintLog("#00bfff", message, obj, size); }

    // Lightシリーズ
    static public void Log_lightcyan(string message, object obj=null, int size=0)   { if (LOG_LIGHT_CYAN > 0) PrintLog("#b0ffff", message, obj, size); }  

    static public void Log_lightyellow(string message, object obj=null, int size=0) { if (LOG_LIGHT_YELLOW > 0) PrintLog("#ffffb0", message, obj, size); }  

    static public void Log_lightmagenta(string message, object obj=null, int size=0){ if (LOG_LIGHT_MAGENTA > 0) PrintLog("#ffb0ff", message, obj, size); }  

    static public void Log_lightlime(string message, object obj=null, int size=0)   { if (LOG_LIGHT_LIME > 0) PrintLog("#66ff66", message, obj, size); }  

    static public void Log_lightred(string message, object obj=null, int size=0)    { if (LOG_LIGHT_RED > 0) PrintLog("#ff6666", message, obj, size); }  

    static public void Log_lightblue(string message, object obj=null, int size=0)   { if (LOG_LIGHT_BLUE > 0) PrintLog("#6699ff", message, obj, size); }  

    static public void Log_lightorange(string message, object obj=null, int size=0) { if (LOG_LIGHT_ORANGE > 0) PrintLog("#ffa566", message, obj, size); }  

    static public void Log_lightskyblue(string message, object obj=null, int size=0){ if (LOG_LIGHT_SKYBLUE > 0) PrintLog("#66bfff", message, obj, size); }  

 

    static public void Log_ErrorWhenObjectIsNull(string message, object target)
    {
        if (target == null)
        {
            LogError(message + " is null.");
        }
    }

    //
    // 巨大な文字列を分割してログ出力する
    // elementSize : 一度に出力する文字数
    //
    static public void Log_BigTextDataWithSeparate(string text, int elementSize)
    {
        int length = text.Length;


        StringBuilder temp = new StringBuilder ();
        for (int i = 0; i < length; i += elementSize) 
        {
            for (int j = 0; j < elementSize; j++) 
            {
                if (length > j+i) 
                {
                    temp.Append (text [j + i]);
                }
            }
            Debug.Log_yellow ("" + temp.ToString());

            temp = new StringBuilder ();
        }

        float sizeKB = length / 1024.0f;

        Debug.Log_yellow ("target text length = " + length + " bytes.    (" + sizeKB.ToString("F2") + "KB)");
    }


    static public string ConvertToString(System.Object[] array)
    {
        StringBuilder sb = new StringBuilder ();
        
        for (int i = 0; i < array.Length; i++)
        {
            sb.Append( array[i].ToString() );

            if (i + 1 < array.Length)
            {
                sb.Append (", ");
            }
        }

        return sb.ToString ();        
    }

    static public string ConvertToString(bool[] array)
    {
        StringBuilder sb = new StringBuilder ();

        for (int i = 0; i < array.Length; i++)
        {
            sb.Append( array[i].ToString() );

            if (i + 1 < array.Length)
            {
                sb.Append (", ");
            }
        }

        return sb.ToString ();        
    }

    static public string ConvertToString(int[] array)
    {
        StringBuilder sb = new StringBuilder ();

        for (int i = 0; i < array.Length; i++)
        {
            sb.Append( array[i].ToString() );

            if (i + 1 < array.Length)
            {
                sb.Append (", ");
            }
        }

        return sb.ToString ();  
    }

    static public string ConvertToString(uint[] array)
    {
        StringBuilder sb = new StringBuilder ();

        for (int i = 0; i < array.Length; i++)
        {
            sb.Append( array[i].ToString() );

            if (i + 1 < array.Length)
            {
                sb.Append (", ");
            }
        }

        return sb.ToString ();  
    }

    static public string ConvertToString(List<int> list)
    {
        StringBuilder sb = new StringBuilder ();

        if (list == null)
        {
            sb.Append("{}");
        }
        else
        {
            sb.Append ("count = [" + list.Count + "], ");
            
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append( list[i].ToString() );

                if (i + 1 < list.Count)
                {
                    sb.Append (", ");
                }
            }
        }

        return sb.ToString ();  
    }


}
