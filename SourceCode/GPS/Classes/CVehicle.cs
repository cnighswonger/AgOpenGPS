﻿//Please, if you use this, share the improvements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL;

namespace AgOpenGPS
{
    public class CVehicle
    {
        private OpenGL gl;
        private FormGPS mf;

        public double toolWidth;
        public double toolFarLeftPosition = 0;
        public double toolFarLeftSpeed = 0;
        public double toolFarRightPosition = 0;
        public double toolFarRightSpeed = 0;

        public double toolOverlap;
        public double toolTrailingHitchLength;
        public double toolOffset;

        public double toolTurnOffDelay;
        public double toolLookAhead;
 
        public bool isToolTrailing;        
        public bool isToolBehindPivot;
        public bool isSteerAxleAhead;

        //vehicle specific
        public bool isPivotBehindAntenna;

        public double antennaHeight;
        public double antennaPivot;
        public double wheelbase;
        public double hitchLength;

        //how many individual sections
        public int numOfSections;
        public int numSuperSection;
        public int minUnappliedPixels = 20;
        public bool areAllSectionsRequiredOn;
        public bool areAllSectionBtnsOn = true;

        //read pixel values
        public int rpXPosition;
        public int rpWidth;

        //min vehicle speed allowed before turning shit off
        public double slowSpeedCutoff = 0;


        public CVehicle(OpenGL gl, FormGPS f)
        {
            //constructor
            this.gl = gl;
            this.mf = f;

            //from settings grab the vehicle specifics
            toolWidth = Properties.Settings.Default.setVehicle_toolWidth;
            toolOverlap = Properties.Settings.Default.setVehicle_toolOverlap;
            toolTrailingHitchLength = Properties.Settings.Default.setVehicle_toolTrailingHitchLength;
            toolOffset = Properties.Settings.Default.setVehicle_toolOffset;

            isToolBehindPivot = Properties.Settings.Default.setVehicle_isToolBehindPivot;            
            isToolTrailing = Properties.Settings.Default.setVehicle_isToolTrailing;

            isPivotBehindAntenna = Properties.Settings.Default.setVehicle_isPivotBehindAntenna;
            antennaHeight = Properties.Settings.Default.setVehicle_antennaHeight;
            antennaPivot = Properties.Settings.Default.setVehicle_antennaPivot;
            hitchLength = Properties.Settings.Default.setVehicle_hitchLength;

            wheelbase = Properties.Settings.Default.setVehicle_wheelbase;
            isSteerAxleAhead = Properties.Settings.Default.setVehicle_isSteerAxleAhead;

            toolLookAhead = Properties.Settings.Default.setVehicle_lookAhead;
            toolTurnOffDelay = Properties.Settings.Default.setVehicle_turnOffDelay;

            numOfSections = Properties.Settings.Default.setVehicle_numSections;
            numSuperSection = numOfSections+1;

            slowSpeedCutoff = Properties.Settings.Default.setVehicle_slowSpeedCutoff;
        }

        public void DrawVehicle()
        {
            //translate and rotate at pivot axle
            gl.Translate(mf.fixPosX, 0, mf.fixPosZ);
            gl.PushMatrix();
            
            //most complicated translate ever!
            gl.Translate((Math.Sin(mf.fixHeading) * (hitchLength - antennaPivot)),0,(Math.Cos(mf.fixHeading) * (hitchLength - antennaPivot)));
            gl.Rotate(glm.toDegrees(mf.fixHeadingSection), 0.0, 1.0, 0.0);
            
            //settings doesn't change trailing hitch length if set to rigid, so do it here
            double trailing;
            if (isToolTrailing) trailing = toolTrailingHitchLength;
            else trailing = 0;
            
            //draw the sections
            gl.LineWidth(4);
            gl.Begin(OpenGL.GL_LINES);

                //draw section line
            if (mf.section[numOfSections].isSectionOn)
            {
                gl.Color(0.97f, 0.297f, 0.97f);
                gl.Vertex(mf.section[numOfSections].positionLeft, 0, trailing);
                gl.Vertex(mf.section[numOfSections].positionRight, 0, trailing);
            }

            else
            for (int j = 0; j < mf.vehicle.numOfSections; j++)
            {
                //if section is on green, if off red color
                if (mf.section[j].isSectionOn)
                {
                    if (mf.section[j].manBtnState == AgOpenGPS.FormGPS.manBtn.Auto) gl.Color(0.0f, 0.97f, 0.0f);
                    else gl.Color(0.99, 0.99, 0);
                }
                else gl.Color(0.97f, 0.2f, 0.2f);

                //draw section line
                gl.Vertex(mf.section[j].positionLeft, 0, trailing);
                gl.Vertex(mf.section[j].positionRight, 0, trailing);
            }
            gl.End();

            //draw the hitch
            gl.LineWidth(2);
            gl.Color(0.0f, 0.0f, 0.0f);

            gl.Begin(OpenGL.GL_LINES);
                gl.Vertex(0, 0, trailing);
                gl.Vertex(0, 0, 0);
            gl.End();

            if (mf.camera.camSetDistance > -2000)
            {
                //section markers
                gl.PointSize(4.0f);
                gl.Begin(OpenGL.GL_POINTS);
                for (int j = 0; j < mf.vehicle.numOfSections - 1; j++)
                    gl.Vertex(mf.section[j].positionRight, 0, trailing);
                gl.End(); 
            }
 
            //draw vehicle
            gl.PopMatrix();
            gl.Rotate(glm.toDegrees(mf.fixHeading), 0.0, 1.0, 0.0);
                        
            //draw the vehicle Body
            gl.Color(0.9, 0.5, 0.30);
            gl.Begin(OpenGL.GL_TRIANGLE_FAN);

                gl.Vertex(0, -0.2, 0);
                gl.Vertex(1.8, 0.0, -antennaPivot);
                gl.Vertex(0, 0.0, -antennaPivot+wheelbase);
                gl.Color(0.20, 0.0, 0.9);
                gl.Vertex(-1.8, 0.0, -antennaPivot);
                gl.Vertex(1.8, 0.0, -antennaPivot);
            gl.End();

           //draw the area side marker
            gl.Color(0.95f, 0.90f, 0.0f);
            gl.PointSize(4.0f);
            gl.Begin(OpenGL.GL_POINTS);
            if (mf.isAreaOnRight) gl.Vertex(2.0, 0, -antennaPivot);
            else gl.Vertex(-2.0, 0, -antennaPivot);


            ////antenna
            //gl.Color(0.0f, 0.98f, 0.0f);
            //gl.Vertex(0, 0, 0);

            //hitch pin
            gl.Color(0.99f, 0.0f, 0.0f);
            gl.Vertex(0, 0, mf.vehicle.hitchLength - antennaPivot);

            ////rear Tires
            //gl.PointSize(12.0f);
            //gl.Color(0, 0, 0);
            //gl.Vertex(-1.8, 0, -antennaPivot);
            //gl.Vertex(1.8, 0, -antennaPivot);
 


            gl.End();
            gl.LineWidth(1);

        }
    }
}

