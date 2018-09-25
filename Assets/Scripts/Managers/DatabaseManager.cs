using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : Singleton<DatabaseManager>
{

    public List<PointStruct> points;

    public TextAsset backupCSV;


    // TODO: We can add a lot of information here. I'd like to think of a better way to show all the info though, all I have now is just a few text objects to show the user.
    // Should also get min and mid points for use with scale factors.
    private int _totalCount;
    private int _class0Count;
    private int _class1Count;
    private int _class2Count;
    private int _maxSL;
    private int _maxSW;
    private int _maxPL;
    private int _maxPW;
    private int _minSL;
    private int _minSW;
    private int _minPL;
    private int _minPW;
    private float _midSL;
    private float _midSW;
    private float _midPL;
    private float _midPW;



    // Read the info
    public int TotalCount { get { return _totalCount; } }
    public int Class0Count { get { return _class0Count; } }
    public int Class1Count { get { return _class1Count; } }
    public int Class2Count { get { return _class2Count; } }
    public int MaxSL { get { return _maxSL; } }
    public int MaxSW { get { return _maxSW; } }
    public int MaxPL { get { return _maxPL; } }
    public int MaxPW { get { return _maxPW; } }
    public int MinSL { get { return _minSL; } }
    public int MinSW { get { return _minSW; } }
    public int MinPL { get { return _minPL; } }
    public int MinPW { get { return _minPW; } }
    public float MidSL { get { return _midSL; } }
    public float MidSW { get { return _midSW; } }
    public float MidPL { get { return _midPL; } }
    public float MidPW { get { return _minPW; } }


    // TODO: this function will only work with the current format of the sample CSV.  It could be modified along with the struct to be a more malleable class, taking the first line and making 
    // members of those name to load the rest. For time's sake, I'm making it a bit rigid here.
    public void LoadCSV(string csv)
    {
        //Debug.Log("Loading CSV: " + csv);

        // check if there was no response from the server, if so, load the csv from the file
        if (csv == "noconnection")
        {
            csv = backupCSV.text;
        }

        //Clear the current points list
        points = new List<PointStruct>();

        // Seperate the lines.
        string[] lineItems = csv.Split("\r"[0]);


        foreach (string line in lineItems)
        {
            string[] numbers = line.Split(',');

            // Tryparse all the numbers for loading into the pointstruct.
            int sepalL = 0;
            int sepalW = 0;
            int petalL = 0;
            int petalW = 0;
            int classy = 0;

            int.TryParse(numbers[0], out sepalL);
            int.TryParse(numbers[1], out sepalW);
            int.TryParse(numbers[2], out petalL);
            int.TryParse(numbers[3], out petalW);
            int.TryParse(numbers[4], out classy);


            // If all are still 0 we should ignore, it's bunk data.
            if (sepalL == 0 && sepalW == 0 && petalL == 0 && petalW == 0 && classy == 0)
            {
               // bunk junk
            }
            else
            {
                // Good data, load it into the points list.
                PointStruct point = new PointStruct(sepalL, sepalW, petalL, petalW, classy);

                points.Add(point);
            }
            
        }

        LoadInfo(); 

        Debug.Log("finished loading points, head out to the range");

        StateManager.Instance.newDataReady();

    }

    // TODO: this shouldn't be a list of gameobjects, I should modify a PointsStruct list when I make changes in game and pass that through. Buuuuuutt shortcut for time.
    public void WriteCSV(List<GameObject> pointsList)
    {
        string csvBuilder = "SepalL,SepalW,PetalL,PetalW,Class";
        foreach (GameObject item in pointsList)
        {
            PointStruct pointInfo = item.GetComponent<Point>().GetPointInfo();
            csvBuilder += "\r";
            csvBuilder += pointInfo.SepalL + ",";
            csvBuilder += pointInfo.SepalW + ",";
            csvBuilder += pointInfo.PetalL + ",";
            csvBuilder += pointInfo.PetalW + ",";
            csvBuilder += pointInfo.Class;
        }

        //Debug.Log(csvBuilder);
        NetworkManager.Instance.SendNewData(csvBuilder);
    }

    private void LoadInfo()
    {
        // Get some variables ready to count up
        int totalCount = 0;
        int class0Count = 0;
        int class1Count = 0;
        int class2Count = 0;

        // Get some max ints ready
        int maxSL = 0;
        int maxSW = 0;
        int maxPL = 0;
        int maxPW = 0;

        // Get some min ints ready
        int minSL = 0;
        int minSW = 0;
        int minPL = 0;
        int minPW = 0;

        // Go through the list only once, add applicable.
        foreach (PointStruct item in points)
        {
            switch (item.Class)
            {
                case 0:
                    class0Count += 1;
                    break;
                case 1:
                    class1Count += 1;
                    break;
                case 2:
                    class2Count += 1;
                    break;
                default:
                    break;
            }
            // Always add one to total NOTE: this could be done (probably faster) by just calling points.Count, I'd have to do some reseach to see how that is handled under the hood in c# though.
            // If it's stored as a variable in the list class like in lua tables, that would be the fastest, if it's just counting through then this way would save a count.
            totalCount += 1;

            // Find maxes
            if (item.SepalL > maxSL)
            {
                maxSL = item.SepalL;
            }
            if (item.SepalW > maxSW)
            {
                maxSW = item.SepalW;
            }
            if (item.PetalL > maxPL)
            {
                maxPL = item.PetalL;
            }
            if (item.PetalW > maxPW)
            {
                maxPW = item.PetalW;
            }

            // Find mins
            if (item.SepalL < minSL)
            {
                minSL = item.SepalL;
            }
            if (item.SepalW < minSW)
            {
                minSW = item.SepalW;
            }
            if (item.PetalL < minPL)
            {
                minPL = item.PetalL;
            }
            if (item.PetalW < minPW)
            {
                minPW = item.PetalW;
            }

        }

        // Assign to the database
        _totalCount = totalCount;
        _class0Count = class0Count;
        _class1Count = class1Count;
        _class2Count = class2Count;
        _maxSL = maxSL;
        _maxSW = maxSW;
        _maxPL = maxPL;
        _maxPW = maxPW;
        _minSL = minSL;
        _minSW = minSW;
        _minPL = minPL;
        _minPW = minPW;

        _midSL = (maxSL + minSL) * .5f;
        _midSW = (maxSW + minSW) * .5f;
        _midPL = (maxPL + minPL) * .5f;
        _midPW = (maxPW + minPW) * .5f;
    }
}

// Point struct for doing calculations on, as well as passing to the Unity gameobjects.
public struct PointStruct
{
    private readonly int _sepalL;
    private readonly int _sepalW;
    private readonly int _petalL;
    private readonly int _petalW;
    private readonly int _class;

    public PointStruct(int sepalL, int sepalW, int petalL, int petalW, int classy)
    {
        _sepalL = sepalL;
        _sepalW = sepalW;
        _petalL = petalL;
        _petalW = petalW;
        _class = classy;
    }

    public int SepalL { get { return _sepalL; } }
    public int SepalW { get { return _sepalW; } }
    public int PetalL { get { return _petalL; } }
    public int PetalW { get { return _petalW; } }
    public int Class { get { return _class; } }
}
