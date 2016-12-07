using UnityEngine;
using System.Collections;

public class RectanglePostProcess : MonoBehaviour
{
    //Color of the line
    public Material BorderMaterial;
    private Vector3 StartingMousePosition = -Vector3.one;
    private Vector3 EndingMousePosition = -Vector3.one;

    void Update()
    {
        UpdateRectangle();
    }

    void UpdateRectangle()
    {
        //Start
        if (Input.GetMouseButtonDown(0)) StartingMousePosition = CalculateMousePosition();
        //Reset
        if (Input.GetMouseButtonUp(0)) StartingMousePosition = -Vector3.one;
        //Held
        if (Input.GetMouseButton(0)) EndingMousePosition = CalculateMousePosition();
    }

    Vector3 CalculateMousePosition()
    {
        float x = Input.mousePosition.x / Screen.width;
        float y = Input.mousePosition.y / Screen.height;

        return new Vector3(x, y, 0);
    }

    public void OnPostRender()
    {
        if (StartingMousePosition == -Vector3.one) return;

        GL.PushMatrix();
        //Apply material
        if (BorderMaterial.SetPass(0))
        {
            //Othographic mode
            GL.LoadOrtho();
            //Start drawing lines
            GL.Begin(GL.LINES);
            {
                //Top
                GL.Vertex(StartingMousePosition);
                GL.Vertex(new Vector3(EndingMousePosition.x, StartingMousePosition.y, 0));

                //Right
                GL.Vertex(new Vector3(EndingMousePosition.x, StartingMousePosition.y, 0));
                GL.Vertex(EndingMousePosition);

                //Bottom
                GL.Vertex(EndingMousePosition);
                GL.Vertex(new Vector3(StartingMousePosition.x, EndingMousePosition.y, 0));

                //Left
                GL.Vertex(new Vector3(StartingMousePosition.x, EndingMousePosition.y, 0));
                GL.Vertex(StartingMousePosition);
            }
            GL.End();
        }
        GL.PopMatrix();
    }
}
