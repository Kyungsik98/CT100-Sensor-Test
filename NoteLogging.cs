using System;
using System.IO;

class Logger
{
    private string logFilePath;

    // 생성자에서 로그 파일 경로 설정
    public Logger(string path)
    {
        logFilePath = path;
    }

    // 로그 기록 메소드
    public void Log(string message)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))  // true는 내용을 추가하는 옵션
            {
                string logEntry = $"{DateTime.Now}: {message}";
                writer.WriteLine(logEntry);  // 로그를 파일에 기록
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"로그 기록 중 오류 발생: {ex.Message}");
        }
    }
}
