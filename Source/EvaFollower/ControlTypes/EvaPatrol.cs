﻿/*
	This file is part of EVA Follower /L Unleashed
		© 2021-2024 LisiasT : http://lisias.net <support@lisias.net>
		© 2014-2016 Marijn Stevens (MSD)
		© 2013 Fel

	EVA Follower /L Unleashed is licensed as follows:
		* CC-BY-NC-SA 3.0 : https://creativecommons.org/licenses/by-nc-sa/3.0/

	EVA Follower /L Unleashed is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvaFollower
{
    /// <summary>
    /// The object responsible for Patroling the kerbal.
    /// </summary>
    class EvaPatrol : IEvaControlType
    {
        public bool AllowRunning { get; set; }
        public List<PatrolAction> actions = new List<PatrolAction>();
        public int currentPatrolPoint = 0;
        public string referenceBody = "None";

        private float delta = 0;

        public bool CheckDistance(double sqrDistance)
        {
            bool complete = (sqrDistance < 0.3);

            if (complete)
            {
                PatrolAction currentPoint = actions[currentPatrolPoint];

                if (currentPoint.type == PatrolActionType.Wait)
                {
                    delta += Time.deltaTime;

                    if (delta > currentPoint.delay)
                    {
                        SetNextPoint();
                        delta = 0;
                    }
                }
                else //move
                {
                    SetNextPoint();
                }
            }

            return complete;
        }

        private void SetNextPoint()
        {
            ++currentPatrolPoint;

            if (currentPatrolPoint >= actions.Count)
                currentPatrolPoint = 0;
        }

        public Vector3d GetNextTarget()
        {
            PatrolAction currentPoint = actions[currentPatrolPoint];
            return Util.GetWorldPos3DLoad(currentPoint.position);
        }

        public void Move(Vessel vessel)
        {
            SetReferenceBody();

            Vector3d position = Util.GetWorldPos3DSave(vessel);
            actions.Add(new PatrolAction(PatrolActionType.Move, 0, position));

            if(EvaSettings.Instance.displayDebugLines)
                setLine(position);

        }

        public void Wait(Vessel vessel)
        {
            SetReferenceBody();

            Vector3d position = Util.GetWorldPos3DSave(vessel);
            actions.Add(new PatrolAction(PatrolActionType.Wait, 1, position));

            if (EvaSettings.Instance.displayDebugLines)
                setLine(position);

        }

        private void SetReferenceBody()
        {
            if (this.referenceBody == "None")
            {
                this.referenceBody = FlightGlobals.ActiveVessel.mainBody.bodyName;
            }
        }

        public void Clear()
        {
            referenceBody = "None";

            currentPatrolPoint = 0;
            actions.Clear();

			this.Hide();
        }

		public void Hide()
		{
			if (null == lineRenderer) return;
			lineRenderer.SetVertexCount(0);
			lineRenderer.enabled = false;
		}

		public string ToSave()
        {
            string actionList = "{";
            for (int i = 0; i < actions.Count-1; i++)
			{
                actionList += actions[i].ToSave();
			}
            actionList += "}";

            string[] args = new string[] {
                AllowRunning.ToString(),
                currentPatrolPoint.ToString(),
                referenceBody.ToString(),
                actionList
            };

            return string.Format("({0}, {1}, {2}, {3})", args);
        }

		public void FromSave(string patrol)
        {
            try
            {
                Log.trace("Patrol.FromSave()");
                EvaTokenReader reader = new EvaTokenReader(patrol);

                string sAllowRunning = reader.NextTokenEnd(',');
                string sCurrentPatrolPoint = reader.NextTokenEnd(',');
                string sReferenceBody = reader.NextTokenEnd(',');
                string sPointlist = reader.NextToken('{', '}');

                AllowRunning = bool.Parse(sAllowRunning);
                currentPatrolPoint = int.Parse(sCurrentPatrolPoint);
                referenceBody = sReferenceBody;

                actions.Clear();

                if (!string.IsNullOrEmpty(sPointlist))
                {
                    reader = new EvaTokenReader(sPointlist);

                    while (!reader.EOF)
                    {
                        PatrolAction action = new PatrolAction();

                        string token = reader.NextToken('(', ')');
                        action.FromSave(token);

                        actions.Add(action);
                    }


                    if (EvaSettings.Instance.displayDebugLines)
                        GenerateLine();
                }
            }
            catch
            {
                throw new Exception("Patrol.FromSave Failed.");
            }
        }


        public void GenerateLine()
        {
            lineRenderer.SetVertexCount(actions.Count+1);

            for (int i = 0; i < actions.Count; i++)
                lineRenderer.SetPosition(i, Util.GetWorldPos3DLoad(actions[i].position));

            lineRenderer.SetPosition(actions.Count, Util.GetWorldPos3DLoad(actions[0].position));
            lineRenderer.enabled = true;
        }


        LineRenderer lineRenderer;

        private void setLine(Vector3d position)
        {
            lineRenderer.SetVertexCount(actions.Count);
            lineRenderer.SetPosition(actions.Count - 1, Util.GetWorldPos3DLoad(position));
        }

        public EvaPatrol()
        {
            if (EvaSettings.Instance.displayDebugLines)
            {
                lineRenderer = new GameObject().AddComponent<LineRenderer>();

                lineRenderer.useWorldSpace = false;
                lineRenderer.material = new Material(Shader.Find(Const.SHADER_PARTICLE_ADDITIVE));
                lineRenderer.SetWidth(0.05f, 0.05f);
                lineRenderer.SetColors(Color.green, Color.red);

				Renderer _renderer = null;
				lineRenderer.GetComponentCached<Renderer> (ref _renderer);

				if (_renderer != null) {
					_renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
					_renderer.receiveShadows = false;
					_renderer.enabled = true;
				}
                lineRenderer.SetVertexCount(0);
            }
        }
    }

    internal class PatrolAction
    {
        public Vector3d position;
        public PatrolActionType type;
        public int delay = 0;

        public PatrolAction()
        {
            this.type = PatrolActionType.Move;
            this.delay = 10;
            this.position = new Vector3d();
        }

        public PatrolAction(PatrolActionType type, int delay, Vector3d position)
        {
            this.type = type;
            this.delay = delay;
            this.position = position;
        }

        internal string ToSave()
        {
            return "(" + type.ToString() + "," + delay.ToString() + "," + position.ToString() +  ")";
        }

        internal void FromSave(string action)
        {
            EvaTokenReader reader = new EvaTokenReader(action);

            string sType = reader.NextTokenEnd(',');
            string sDelay = reader.NextTokenEnd(',');
            string sPosition = reader.NextToken('[', ']');

            type = (PatrolActionType)Enum.Parse(typeof(PatrolActionType), sType);
            delay = int.Parse(sDelay);
            position = Util.ParseVector3d(sPosition, false);
        }

        public override string ToString()
        {
            return "position = " + position.ToString() + ", delay = " + delay + ", type = " + type.ToString();
        }
    }

    [Flags]
    internal enum PatrolActionType
    {
        Move,
        Wait,
    }

}
