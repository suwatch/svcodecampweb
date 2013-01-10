public class DataObjectAgendaUpdateInfo
{
    private int lectureRoomId;

    public int LectureRoomId
    {
        get { return lectureRoomId; }
        set { lectureRoomId = value; }
    }

    private int sessionTimesId;

    public int SessionTimesId
    {
        get { return sessionTimesId; }
        set { sessionTimesId = value; }
    }



    private int sessionId;

    public int SessionId
    {
        get { return sessionId; }
        set { sessionId = value; }
    }


    private string sessionTitle;

    public string SessionTitle
    {
        get { return sessionTitle; }
        set { sessionTitle = value; }
    }
    private string sessionAuthor;

    public string SessionAuthor
    {
        get { return sessionAuthor; }
        set { sessionAuthor = value; }
    }
    private int interested;

    public int Interested
    {
        get { return interested; }
        set { interested = value; }
    }
    private int willAttend;

    public int WillAttend
    {
        get { return willAttend; }
        set { willAttend = value; }
    }

}
