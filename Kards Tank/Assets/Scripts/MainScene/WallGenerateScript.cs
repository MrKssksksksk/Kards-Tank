using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerateScript : MonoBehaviour
{
    public GameObject wallPiece;
    public Transform wall;
    public float wallPercent;
    private int[,] points = new int[16, 8];
    private int centerX, centerY;

    /*
     * 
     * 8
     * 7
     * 6
     * 5
     * 4
     * 3
     * 2
     * 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16
     */

    // 1:right 2:up 3:left 4:down 0:center

    void Start()
    {
        // init setting
        centerX = 15;
        centerY = 7;
        for (int j = 0;j < 8; j += 2)
        {
            for (int i = 0; i < 16; i += 2)
            {
                points[i, j] = 1;
            }
        }
        for (int j = 1; j < 8; j += 2)
        {
            for (int i = 1; i < 15; i += 2)
            {
                points[i, j] = 1;
            }
        }
        for (int j = 0; j < 8; j += 2)
        {
            for (int i = 1; i < 16; i += 2)
            {
                points[i, j] = 2;
            }
        }
        for (int j = 1; j < 7; j += 2)
        {
            for (int i = 0; i < 16; i += 2)
            {
                points[i, j] = 2;
            }
        }
        for (int j = 1; j < 6; j += 2)
        {
            points[15, j] = 3;
        }
        for (int i = 0; i < 16; i += 2)
        {
            points[i, 7] = 4;
        }
        points[15, 7] = 0;

        // randomize the maze
        for (int i = 0; i < 1000; i++) randomizeMaze();

        // output
        for (int i = 0; i < 16;  i++)
        {
            for (int j = 0;j < 8; j++)
            {
                // for each point, only check right and up to prevent repeat
                if (i != 15 && points[i, j] != 1 && points[i + 1, j] != 3)
                {
                    if (UnityEngine.Random.Range(1, 101) <= wallPercent * 100) generateWallOn(i, j, 1);
                }
                if (j != 7 && points[i, j] != 2 && points[i, j + 1] != 4)
                {
                    if (UnityEngine.Random.Range(1, 101) <= wallPercent * 100) generateWallOn(i, j, 2);
                }
            }
        }
    }

    void randomizeMaze()
    {
        int r;
        int nextX, nextY;
        while (true)
        {
            r = UnityEngine.Random.Range(1, 5); // random 1~4
            // Debug.Log("r: " + r);
            if (r == 1 && centerX != 15)
            {
                nextX = centerX + 1;
                nextY = centerY;
                break;
            }
            else if (r == 3 && centerX != 0)
            {
                nextX = centerX - 1;
                nextY = centerY;
                break;
            }
            else if (r == 2 && centerY != 7)
            {
                nextX = centerX;
                nextY = centerY + 1;
                break;
            }
            else if (r == 4 && centerY != 0)
            {
                nextX = centerX;
                nextY = centerY - 1;
                break;
            }
        }
        points[nextX, nextY] = 0;
        points[centerX, centerY] = r;
        // Debug.Log(centerX + " " + centerY + " -> " + nextX + " " + nextY);
        centerX = nextX;
        centerY = nextY;
    }

    void generateWallOn(int ix, int iy, int r)
    {
        // r=1:right r=2:up r=3:left r=4:down
        float x, y;
        Quaternion rotation;
        if (r == 1)
        {
            x = -7f + ix;
            y = -3.5f + iy;
            rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (r == 3)
        {
            x = -8f + ix;
            y = -3.5f + iy;
            rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (r == 2)
        {
            x = -7.5f + ix;
            y = -3f + iy;
            rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            x = -7.5f + ix;
            y = -4f + iy;
            rotation = Quaternion.Euler(0, 0, 0);
        }
        Instantiate(wallPiece, new Vector3(x, y, -6.547767f), rotation, wall);
    }
}
