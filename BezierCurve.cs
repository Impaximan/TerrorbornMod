using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TerrorbornMod
{
    public class BezierCurve // Bezier Curve Method, made by Seraph
	{
		public List<Vector2> Controls; // the points used for the "verticies" of the curver
		public BezierCurve(params Vector2[] controlPoints)
		{
			List<Vector2> points = new List<Vector2>();
			for (int i = 0; i < controlPoints.Length; ++i)
			{
				points.Add(controlPoints[i]);
			}
			Controls = points;
		}

		public BezierCurve(List<Vector2> controlPoints)
		{

			Controls = controlPoints;
		}



		private Vector2 EvaluateRecursive(List<Vector2> controls, float distance)
		{
			if (controls.Count <= 2)
			{

				return Vector2.Lerp(controls[0], controls[1], distance);

			}
			else
			{
				List<Vector2> nextPoints = new List<Vector2>();

				for (int i = 0; i < controls.Count - 1; ++i)
				{
					nextPoints.Add(Vector2.Lerp(controls[i], controls[i + 1], distance));
				}

				return EvaluateRecursive(nextPoints, distance);
			}


		}

		public Vector2 GetSinglePoint(float proportionalDistance)
		{
			if (proportionalDistance > 1f)
			{
				proportionalDistance = 1f;
			}
			if (proportionalDistance < 0f)
			{
				proportionalDistance = 0f;
			}

			return EvaluateRecursive(Controls, proportionalDistance);
		}

		public List<Vector2> GetPoints(int amount)//returns a list
		{
			float interval = 1f / (float)amount;


			List<Vector2> points = new List<Vector2>();

			for (float i = 0f; i <= 1f; i += interval)
			{
				Vector2 point = GetSinglePoint(i);
				points.Add(point);
			}

			return points;
		}

		public List<Point> GetPointsRounded(int amount)//returns a list
		{
			float interval = 1f / (float)amount;


			List<Point> points = new List<Point>();

			for (float i = 0f; i <= 1f; i += interval)
			{
				Vector2 pointBase = GetSinglePoint(i);
				Point point = new Point((int)Math.Round(pointBase.X), (int)Math.Round(pointBase.Y));
				points.Add(point);
			}

			return points;
		}

		public Vector2[] GetPointsAlternate(int amount)//returns an array
		{
			float interval = 1f / (float)amount;


			Vector2[] points = new Vector2[amount];

			for (float i = 0f; i <= 1f; i += interval)
			{
				int index = (int)((float)i / interval);
				points[index] = GetSinglePoint(i);
			}

			return points;
		}


	}
}

