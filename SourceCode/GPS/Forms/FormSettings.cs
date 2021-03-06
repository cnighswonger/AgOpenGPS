﻿//Please, if you use this, share the improvements


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSettings : Form
    {
       //class variables
        private FormGPS mf = null;

        double toolOverlap, toolTrailingHitchLength, toolOffset, toolTurnOffDelay, toolLookAhead;
        double antennaHeight, antennaPivot, wheelbase, hitchLength, pitchZeroSet, rollZeroSet;

        bool isToolTrailing, isToolBehindPivot, isPivotBehindAntenna, isSteerAxleAhead, isAtanCamera;
        int numberOfSections;

        decimal sectionWidth1, sectionWidth2, sectionWidth3, sectionWidth4, sectionWidth5, sectionWidth6, sectionWidth7, sectionWidth8;
        decimal sectionPosition1, sectionPosition2, sectionPosition3, sectionPosition4,
                    sectionPosition5, sectionPosition6, sectionPosition7, sectionPosition8, sectionPosition9;

        decimal triResolution, minFixStepDistance;

        bool isWorkSwEn, isWorkSwActiveLow;

        double metImp2m, m2MetImp, cutoffMetricImperial, cutoffSpeed, maxWidth;

        //string gooleEarthPath;

        //constructor
        public FormSettings(Form callingForm, int page)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            if (mf.isMetric)
            {
                metImp2m = 0.01;
                m2MetImp = 100.0;
                lblInchesCm.Text = "Centimeters";
                lblSecTotalWidthFeet.Visible = false;
                lblSecTotalWidthInches.Visible = false;
                lblSecTotalWidthMeters.Visible = true;
                lblDoNotExceed.Text = "* Do not exceed 4000 cm*";
                lblTurnOffBelowUnits.Text = "Km/h";
                cutoffMetricImperial = 1;
                maxWidth = 4000;
            }

            else
            {
                metImp2m = glm.in2m;
                m2MetImp = glm.m2in;
                lblInchesCm.Text = "Inches";
                lblSecTotalWidthFeet.Visible = true;
                lblSecTotalWidthInches.Visible = true;
                lblSecTotalWidthMeters.Visible = false;
                lblDoNotExceed.Text = "* Do not exceed 1570 inches *";
                lblTurnOffBelowUnits.Text = "MPH";
                cutoffMetricImperial = 1.60934;
                maxWidth = 1570;
            }
            //select the page as per calling menu or button from mainGPS form
            tabControl1.SelectedIndex = page;
        }

        //do any field initializing for form here
        private void FormSettings_Load(object sender, EventArgs e)
        {
            //Vehicle settings to what it is in the settings page------------------------------------------------
            antennaHeight = Properties.Settings.Default.setVehicle_antennaHeight;     
            antennaPivot = Math.Abs(Properties.Settings.Default.setVehicle_antennaPivot);
            hitchLength = Math.Abs(Properties.Settings.Default.setVehicle_hitchLength);
            wheelbase = Math.Abs(Properties.Settings.Default.setVehicle_wheelbase);

            nudAntennaHeight.ValueChanged -= nudAntennaHeight_ValueChanged;
            nudAntennaHeight.Value = (decimal)(antennaHeight * m2MetImp);
            nudAntennaHeight.ValueChanged += nudAntennaHeight_ValueChanged;

            nudAntennaPivot.ValueChanged -= nudAntennaPivot_ValueChanged;
            nudAntennaPivot.Value = (decimal)(antennaPivot * m2MetImp);
            nudAntennaPivot.ValueChanged += nudAntennaPivot_ValueChanged;

            nudHitchLength.ValueChanged -= nudHitchLength_ValueChanged;
            nudHitchLength.Value = (decimal)(hitchLength * m2MetImp);
            nudHitchLength.ValueChanged += nudHitchLength_ValueChanged;

            nudWheelbase.ValueChanged -= nudWheelbase_ValueChanged;
            nudWheelbase.Value = (decimal)(wheelbase * m2MetImp);
            nudWheelbase.ValueChanged += nudWheelbase_ValueChanged;

            
            //Tool    hitched, pivot behind antenna, and tool behind pivot are the default as true------------------------------------------------------
            if (Properties.Settings.Default.setVehicle_isToolBehindPivot) isToolBehindPivot = true;
            else isToolBehindPivot = false;

            if (Properties.Settings.Default.setVehicle_isToolTrailing) isToolTrailing = true;
            else isToolTrailing = false;

            if (Properties.Settings.Default.setVehicle_isPivotBehindAntenna) isPivotBehindAntenna = true;
            else isPivotBehindAntenna = false;

            if (Properties.Settings.Default.setVehicle_isSteerAxleAhead) isSteerAxleAhead = true;
            else isSteerAxleAhead = false;

            chkIsAft.CheckedChanged -= chkIsAft_CheckedChanged;
            chkIsAft.Checked = isToolBehindPivot;
            chkIsAft.CheckedChanged += chkIsAft_CheckedChanged;

            chkIsTrailingRigid.CheckedChanged -= chkIsTrailingRigid_CheckedChanged;
            chkIsTrailingRigid.Checked = isToolTrailing;
            chkIsTrailingRigid.CheckedChanged += chkIsTrailingRigid_CheckedChanged;

            chkIsPivotBehindAntenna.CheckedChanged -= chkIsPivotBehindAntenna_CheckedChanged;
            chkIsPivotBehindAntenna.Checked = isPivotBehindAntenna;
            chkIsPivotBehindAntenna.CheckedChanged += chkIsPivotBehindAntenna_CheckedChanged;

            chkIsSteerAxleAhead.CheckedChanged -= chkIsSteerAxleAhead_CheckedChanged;
            chkIsSteerAxleAhead.Checked = isSteerAxleAhead;
            chkIsSteerAxleAhead.CheckedChanged += chkIsSteerAxleAhead_CheckedChanged;

            UpdateTrailingRigidCheckbox();
            UpdateIsAftCheckbox();
            UpdateIsPivotBehindAntennaCheckbox();
            UpdateIsSteerAxleAhead();

            toolTrailingHitchLength = Math.Abs(Properties.Settings.Default.setVehicle_toolTrailingHitchLength);    

            toolOverlap = Properties.Settings.Default.setVehicle_toolOverlap;
            toolOffset = Properties.Settings.Default.setVehicle_toolOffset;

            toolTurnOffDelay = Properties.Settings.Default.setVehicle_turnOffDelay;
            toolLookAhead = Properties.Settings.Default.setVehicle_lookAhead;


            nudOverlap.ValueChanged -= nudOverlap_ValueChanged;
            nudOverlap.Value = (decimal)(toolOverlap * m2MetImp);
            nudOverlap.ValueChanged += nudOverlap_ValueChanged;

            nudForeAft.ValueChanged -= nudAft_ValueChanged;
            nudForeAft.Value = (decimal)(toolTrailingHitchLength * m2MetImp);
            nudForeAft.ValueChanged += nudAft_ValueChanged;

            nudOffset.ValueChanged -= nudOffset_ValueChanged;
            nudOffset.Value = (decimal)(toolOffset * m2MetImp);
            nudOffset.ValueChanged += nudOffset_ValueChanged;

            nudTurnOffDelay.ValueChanged -= nudTurnOffDelay_ValueChanged;
            nudTurnOffDelay.Value = (decimal)(toolTurnOffDelay);
            nudTurnOffDelay.ValueChanged += nudTurnOffDelay_ValueChanged;

            nudLookAhead.ValueChanged -= nudLookAhead_ValueChanged;
            nudLookAhead.Value = (decimal)(toolLookAhead);
            nudLookAhead.ValueChanged += nudLookAhead_ValueChanged;
            
             //Sections set to settings page ----------------------------------------------------------------------
            numberOfSections = Properties.Settings.Default.setVehicle_numSections;

            //grab number of sections
            nudNumberOfSections.ValueChanged -= nudNumberOfSections_ValueChanged;
            nudNumberOfSections.Value = numberOfSections;
            nudNumberOfSections.ValueChanged += nudNumberOfSections_ValueChanged;

            //calc the 8 section widths based on settings.settings also meters to inches
            nudSection1.Value = Math.Abs((Properties.Settings.Default.setSection_position2 - Properties.Settings.Default.setSection_position1) * (decimal)m2MetImp);
            nudSection2.Value = Math.Abs((Properties.Settings.Default.setSection_position3 - Properties.Settings.Default.setSection_position2) * (decimal)m2MetImp);
            nudSection3.Value = Math.Abs((Properties.Settings.Default.setSection_position4 - Properties.Settings.Default.setSection_position3) * (decimal)m2MetImp);
            nudSection4.Value = Math.Abs((Properties.Settings.Default.setSection_position5 - Properties.Settings.Default.setSection_position4) * (decimal)m2MetImp);
            nudSection5.Value = Math.Abs((Properties.Settings.Default.setSection_position6 - Properties.Settings.Default.setSection_position5) * (decimal)m2MetImp);
            nudSection6.Value = Math.Abs((Properties.Settings.Default.setSection_position7 - Properties.Settings.Default.setSection_position6) * (decimal)m2MetImp);
            nudSection7.Value = Math.Abs((Properties.Settings.Default.setSection_position8 - Properties.Settings.Default.setSection_position7) * (decimal)m2MetImp);
            nudSection8.Value = Math.Abs((Properties.Settings.Default.setSection_position9 - Properties.Settings.Default.setSection_position8) * (decimal)m2MetImp); 

            //based on number of sections and values update the page before displaying
            UpdateSpinners();            

            isAtanCamera = Properties.Settings.Default.setCam_isAtanCam;

            chkIsAtanCam.CheckedChanged -= chkIsAtanCam_CheckedChanged;
            chkIsAtanCam.Checked = isAtanCamera;
            chkIsAtanCam.CheckedChanged += chkIsAtanCam_CheckedChanged;

            triResolution = (decimal)Properties.Settings.Default.setDisplay_triangleResolution;
            nudTriangleResolution.Value = triResolution;

            minFixStepDistance = (decimal)Properties.Settings.Default.set_minFixStep;
            nudMinFixStepDistance.Value = minFixStepDistance;

            isWorkSwActiveLow = Properties.Settings.Default.setIsWorkSwitchActiveLow;

            chkWorkSwActiveLow.CheckedChanged -= chkWorkSwActiveLow_CheckedChanged;
            chkWorkSwActiveLow.Checked = isWorkSwActiveLow;
            chkWorkSwActiveLow.CheckedChanged += chkWorkSwActiveLow_CheckedChanged;

            isWorkSwEn = Properties.Settings.Default.setIsWorkSwitchEnabled;

            chkEnableWorkSwitch.CheckedChanged -= chkEnableWorkSwitch_CheckedChanged;
            chkEnableWorkSwitch.Checked = isWorkSwEn;
            chkEnableWorkSwitch.CheckedChanged += chkEnableWorkSwitch_CheckedChanged;

            pitchZeroSet = Properties.Settings.Default.setIMU_pitchZero;
            rollZeroSet = Properties.Settings.Default.setIMU_rollZero;

            cutoffSpeed = Properties.Settings.Default.setVehicle_slowSpeedCutoff/cutoffMetricImperial;

            nudCutoffSpeed.ValueChanged -= nudCutoffSpeed_ValueChanged;
            nudCutoffSpeed.Value = (decimal)cutoffSpeed;
            nudCutoffSpeed.ValueChanged += nudCutoffSpeed_ValueChanged;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           //Vehicle settings -------------------------------------------------------------------------------

            if (!isPivotBehindAntenna) antennaPivot *= -1;
            mf.vehicle.antennaPivot = antennaPivot;
            Properties.Settings.Default.setVehicle_antennaPivot = mf.vehicle.antennaPivot;            

            if (isPivotBehindAntenna) mf.vehicle.isPivotBehindAntenna = true;
            else mf.vehicle.isPivotBehindAntenna = false;
            Properties.Settings.Default.setVehicle_isPivotBehindAntenna = mf.vehicle.isPivotBehindAntenna;

            if (!isSteerAxleAhead) wheelbase *= -1;
            mf.vehicle.wheelbase = wheelbase;
            Properties.Settings.Default.setVehicle_wheelbase = wheelbase;

            if (isSteerAxleAhead) mf.vehicle.isSteerAxleAhead = true;
            else mf.vehicle.isSteerAxleAhead = false;
            Properties.Settings.Default.setVehicle_isSteerAxleAhead = mf.vehicle.isSteerAxleAhead;

            mf.vehicle.antennaHeight = antennaHeight;
            Properties.Settings.Default.setVehicle_antennaHeight = mf.vehicle.antennaHeight;

            //Tool  ------------------------------------------------------------------------------------------

            //trailing hitch is always behind
            toolTrailingHitchLength *= -1;
            mf.vehicle.toolTrailingHitchLength = toolTrailingHitchLength;
            Properties.Settings.Default.setVehicle_toolTrailingHitchLength = mf.vehicle.toolTrailingHitchLength;

            mf.vehicle.toolLookAhead = toolLookAhead;
            Properties.Settings.Default.setVehicle_lookAhead = mf.vehicle.toolLookAhead;

            mf.vehicle.toolOverlap = toolOverlap;
            Properties.Settings.Default.setVehicle_toolOverlap = mf.vehicle.toolOverlap;

            mf.vehicle.toolTurnOffDelay = toolTurnOffDelay;
            Properties.Settings.Default.setVehicle_turnOffDelay = mf.vehicle.toolTurnOffDelay;

            mf.vehicle.toolOffset = toolOffset;
            Properties.Settings.Default.setVehicle_toolOffset = mf.vehicle.toolOffset;

            if (isToolTrailing) mf.vehicle.isToolTrailing = true;
            else mf.vehicle.isToolTrailing = false;
            Properties.Settings.Default.setVehicle_isToolTrailing = mf.vehicle.isToolTrailing;

            if (isToolBehindPivot) mf.vehicle.isToolBehindPivot = true;
            else mf.vehicle.isToolBehindPivot = false;
            Properties.Settings.Default.setVehicle_isToolBehindPivot = mf.vehicle.isToolBehindPivot;

            if (isToolBehindPivot) hitchLength *= -1;
            mf.vehicle.hitchLength = hitchLength;
            Properties.Settings.Default.setVehicle_hitchLength = mf.vehicle.hitchLength;

            //Sections ------------------------------------------------------------------------------------------

            mf.vehicle.numOfSections = numberOfSections;
            mf.vehicle.numSuperSection = numberOfSections + 1;
            Properties.Settings.Default.setVehicle_numSections = mf.vehicle.numOfSections;

            //take the section widths and convert to meters and positions along tool.
            CalculateSectionPositions();

            //save the values in each spinner for section position widths in settings
            Properties.Settings.Default.setSection_position1 = sectionPosition1;
            Properties.Settings.Default.setSection_position2 = sectionPosition2;
            Properties.Settings.Default.setSection_position3 = sectionPosition3;
            Properties.Settings.Default.setSection_position4 = sectionPosition4;
            Properties.Settings.Default.setSection_position5 = sectionPosition5;
            Properties.Settings.Default.setSection_position6 = sectionPosition6;
            Properties.Settings.Default.setSection_position7 = sectionPosition7;
            Properties.Settings.Default.setSection_position8 = sectionPosition8;
            Properties.Settings.Default.setSection_position9 = sectionPosition9;

            //line up manual buttons based on # of sections
            mf.LineUpManualBtns();

            //update the sections to newly configured widths and positions in main
            mf.SectionSetPosition();

            //update the widths of sections and tool width in main
            mf.SectionCalcWidths();

            //update toolwidth in mainform
            Properties.Settings.Default.setVehicle_toolWidth = mf.vehicle.toolWidth;

            ////Display ---load the delay slides --------------------------------------------------------------------
            mf.isAtanCam = isAtanCamera;
            Properties.Settings.Default.setCam_isAtanCam = isAtanCamera;

            mf.triangleResolution = (double)triResolution;
            Properties.Settings.Default.setDisplay_triangleResolution = mf.triangleResolution;

            mf.minFixStepDist = (double)minFixStepDistance;
            Properties.Settings.Default.set_minFixStep = mf.minFixStepDist;

            mf.modcom.isWorkSwitchActiveLow = isWorkSwActiveLow;
            Properties.Settings.Default.setIsWorkSwitchActiveLow = isWorkSwActiveLow;

            mf.modcom.isWorkSwitchEnabled = isWorkSwEn;
            Properties.Settings.Default.setIsWorkSwitchEnabled = isWorkSwEn;

            mf.rollZero = rollZeroSet;
            mf.pitchZero = pitchZeroSet;

            Properties.Settings.Default.setIMU_rollZero = rollZeroSet;
            Properties.Settings.Default.setIMU_pitchZero = pitchZeroSet;

            Properties.Settings.Default.setVehicle_slowSpeedCutoff = (double)cutoffSpeed*cutoffMetricImperial;
            mf.vehicle.slowSpeedCutoff = (double)cutoffSpeed*cutoffMetricImperial;

            Properties.Settings.Default.Save();

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        { DialogResult = DialogResult.Cancel; Close(); }

        #region Vehicle //----------------------------------------------------------------


        private void nudAntennaHeight_ValueChanged(object sender, EventArgs e)
        {
            antennaHeight = (double)nudAntennaHeight.Value * metImp2m;
        }

        private void nudAntennaPivot_ValueChanged(object sender, EventArgs e)
        {
            antennaPivot = (double)nudAntennaPivot.Value * metImp2m;
        }

        private void nudHitchLength_ValueChanged(object sender, EventArgs e)
        {
            hitchLength = (double)nudHitchLength.Value * metImp2m;
        }

        private void nudWheelbase_ValueChanged(object sender, EventArgs e)
        {
            wheelbase = (double)nudWheelbase.Value * metImp2m;
        }

        private void btnFileOpenVehicle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            mf.FileOpenVehicle();
        }

        private void btnFileSaveVehicle_Click(object sender, EventArgs e)
        {
            mf.FileSaveVehicle();
        }

 
 
 
        private void UpdateTrailingRigidCheckbox()
        {
            //change 2D or 3D icon accordingly on button
            if (chkIsTrailingRigid.Checked) {
                this.chkIsTrailingRigid.Image = global::AgOpenGPS.Properties.Resources.VehHitched64; }
            else  {  this.chkIsTrailingRigid.Image = global::AgOpenGPS.Properties.Resources.VehRigid64; }
        }

       private void UpdateIsAftCheckbox()
        {
            if (chkIsAft.Checked)
            {
                chkIsTrailingRigid.Enabled = true;
                nudForeAft.Enabled = true;
                this.chkIsAft.Image = global::AgOpenGPS.Properties.Resources.VehAft64; 
            }
            else
            {
                chkIsTrailingRigid.Checked = false;
                nudForeAft.Enabled = false;
                isToolTrailing = false;
                UpdateTrailingRigidCheckbox();
                chkIsTrailingRigid.Enabled = false;
                this.chkIsAft.Image = global::AgOpenGPS.Properties.Resources.VehFore64; 
            }
        }

       private void UpdateIsPivotBehindAntennaCheckbox()
       {
           if (chkIsPivotBehindAntenna.Checked) {
               this.chkIsPivotBehindAntenna.Image = global::AgOpenGPS.Properties.Resources.PivotBehind;
           }
           else { this.chkIsPivotBehindAntenna.Image = global::AgOpenGPS.Properties.Resources.PivotAhead; }

       }

        private void UpdateIsSteerAxleAhead()
        {
            if (chkIsSteerAxleAhead.Checked)
            {
                this.chkIsSteerAxleAhead.Image = global::AgOpenGPS.Properties.Resources.SteerAhead;
            }
            else { this.chkIsSteerAxleAhead.Image = global::AgOpenGPS.Properties.Resources.SteerBehind; }
        }

        private void chkIsTrailingRigid_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTrailingRigidCheckbox();
            isToolTrailing = !isToolTrailing;
        }
      
        private void chkIsAft_CheckedChanged(object sender, EventArgs e)
        {
            UpdateIsAftCheckbox(); 
            isToolBehindPivot = !isToolBehindPivot;

        }

         private void chkIsPivotBehindAntenna_CheckedChanged(object sender, EventArgs e)
        {
            isPivotBehindAntenna = !isPivotBehindAntenna;
            UpdateIsPivotBehindAntennaCheckbox();

        }

        private void chkIsSteerAxleAhead_CheckedChanged(object sender, EventArgs e)
        {
            isSteerAxleAhead = !isSteerAxleAhead;
            UpdateIsSteerAxleAhead();

        }


        private void nudAft_ValueChanged(object sender, EventArgs e)
        {
            toolTrailingHitchLength = (double)(nudForeAft.Value) * metImp2m;
        }

        private void nudOffset_ValueChanged(object sender, EventArgs e)
        {
            toolOffset = (double)nudOffset.Value * metImp2m;
        }

        private void nudLookAhead_ValueChanged(object sender, EventArgs e)
        {
            toolLookAhead = (double)nudLookAhead.Value;
        }

        private void nudTurnOffDelay_ValueChanged(object sender, EventArgs e)
        {
            toolTurnOffDelay = (double)nudTurnOffDelay.Value;
        }

        private void nudOverlap_ValueChanged(object sender, EventArgs e)
        {
            toolOverlap = (double)nudOverlap.Value * metImp2m; 
        }

       #endregion Vehicle

        #region Sections //---------------------------------------------------------------


        //enable or disable section width spinners based on number sections selected
        public void UpdateSpinners()
        {
            int i = (int)nudNumberOfSections.Value;
            switch (i)
            {
                case 1:
                    {
                        nudSection1.Enabled = true;  nudSection1.Visible = true;
                        nudSection2.Enabled = false; nudSection2.Visible = false;
                        nudSection3.Enabled = false; nudSection3.Visible = false;
                        nudSection4.Enabled = false; nudSection4.Visible = false;
                        nudSection5.Enabled = false; nudSection5.Visible = false;
                        nudSection6.Enabled = false; nudSection6.Visible = false;
                        nudSection7.Enabled = false; nudSection7.Visible = false;
                        nudSection8.Enabled = false; nudSection8.Visible = false;

                        nudSection1.Left = 432;

                        lblVehicleToolWidth.Text = Convert.ToString((int)nudSection1.Value);
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings1;

                        break;
                    }
                case 2:
                    {
                        nudSection1.Enabled = true; nudSection1.Visible = true;
                        nudSection2.Enabled = true; nudSection2.Visible = true;
                        nudSection3.Enabled = false; nudSection3.Visible = false;
                        nudSection4.Enabled = false; nudSection4.Visible = false;
                        nudSection6.Enabled = false; nudSection6.Visible = false;
                        nudSection7.Enabled = false; nudSection7.Visible = false;
                        nudSection8.Enabled = false; nudSection8.Visible = false;
                        nudSection5.Enabled = false; nudSection5.Visible = false;

                        nudSection1.Left = 182;
                        nudSection2.Left = 681;

                        lblVehicleToolWidth.Text = Convert.ToString((int)(nudSection1.Value + nudSection2.Value));
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings2;
                        break;
                    }

                case 3:
                    {
                        nudSection1.Enabled = true; nudSection1.Visible = true;
                        nudSection2.Enabled = true; nudSection2.Visible = true;
                        nudSection3.Enabled = true; nudSection3.Visible = true;
                        nudSection4.Enabled = false; nudSection4.Visible = false;
                        nudSection5.Enabled = false; nudSection5.Visible = false;
                        nudSection6.Enabled = false; nudSection6.Visible = false;
                        nudSection7.Enabled = false; nudSection7.Visible = false;
                        nudSection8.Enabled = false; nudSection8.Visible = false;

                        nudSection1.Left = 130;
                        nudSection2.Left = 433;
                        nudSection3.Left = 733;

                        lblVehicleToolWidth.Text = Convert.ToString((int)(nudSection1.Value+nudSection2.Value+nudSection3.Value));
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings3;
                        break;
                    }

                case 4:
                    {
                        nudSection1.Enabled = true; nudSection1.Visible = true;
                        nudSection2.Enabled = true; nudSection2.Visible = true;
                        nudSection3.Enabled = true; nudSection3.Visible = true;
                        nudSection4.Enabled = true; nudSection4.Visible = true;
                        nudSection5.Enabled = false; nudSection5.Visible = false;
                        nudSection6.Enabled = false; nudSection6.Visible = false;
                        nudSection7.Enabled = false; nudSection7.Visible = false;
                        nudSection8.Enabled = false; nudSection8.Visible = false;
                        nudSection1.Left = 75;
                        nudSection2.Left = 309;
                        nudSection3.Left = 551;
                        nudSection4.Left = 781;

                        lblVehicleToolWidth.Text = Convert.ToString((int)(nudSection1.Value+nudSection2.Value+nudSection3.Value+nudSection4.Value));
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings4;
                        break;
                    }

                case 5:
                    {
                        nudSection1.Enabled = true; nudSection1.Visible = true;
                        nudSection2.Enabled = true; nudSection2.Visible = true;
                        nudSection3.Enabled = true; nudSection3.Visible = true;
                        nudSection4.Enabled = true; nudSection4.Visible = true;
                        nudSection5.Enabled = true; nudSection5.Visible = true;
                        nudSection6.Enabled = false; nudSection6.Visible = false;
                        nudSection7.Enabled = false; nudSection7.Visible = false;
                        nudSection8.Enabled = false; nudSection8.Visible = false;
                        nudSection1.Left = 55;
                        nudSection2.Left = 239;
                        nudSection3.Left = 427;
                        nudSection4.Left = 615;
                        nudSection5.Left = 803;

                        lblVehicleToolWidth.Text = Convert.ToString((int)(nudSection1.Value + nudSection2.Value + nudSection3.Value + nudSection4.Value + nudSection5.Value));
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings5;
                        break;
                    }

                case 6:
                    {
                        nudSection1.Enabled = true; nudSection1.Visible = true;
                        nudSection2.Enabled = true; nudSection2.Visible = true;
                        nudSection3.Enabled = true; nudSection3.Visible = true;
                        nudSection4.Enabled = true; nudSection4.Visible = true;
                        nudSection5.Enabled = true; nudSection5.Visible = true;
                        nudSection6.Enabled = true; nudSection6.Visible = true;
                        nudSection7.Enabled = false; nudSection7.Visible = false;
                        nudSection8.Enabled = false; nudSection8.Visible = false;
                        nudSection1.Left = 45;
                        nudSection2.Left = 198;
                        nudSection3.Left = 353;
                        nudSection4.Left = 510;
                        nudSection5.Left = 669;
                        nudSection6.Left = 821;

                        lblVehicleToolWidth.Text = Convert.ToString((int)(nudSection1.Value + nudSection2.Value + nudSection3.Value + nudSection4.Value +
                                nudSection5.Value + nudSection6.Value));
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings6;
                        break;
                    }

                case 7:
                    {
                        nudSection1.Enabled = true; nudSection1.Visible = true;
                        nudSection2.Enabled = true; nudSection2.Visible = true;
                        nudSection3.Enabled = true; nudSection3.Visible = true;
                        nudSection4.Enabled = true; nudSection4.Visible = true;
                        nudSection5.Enabled = true; nudSection5.Visible = true;
                        nudSection6.Enabled = true; nudSection6.Visible = true;
                        nudSection7.Enabled = true; nudSection7.Visible = true;
                        nudSection8.Enabled = false; nudSection8.Visible = false;
                        nudSection1.Left = 29;
                        nudSection2.Left = 162;
                        nudSection3.Left = 296;
                        nudSection4.Left = 427;
                        nudSection5.Left = 561;
                        nudSection6.Left = 696;
                        nudSection7.Left = 830;

                        lblVehicleToolWidth.Text = Convert.ToString((int)(nudSection1.Value + nudSection2.Value + nudSection3.Value +
                            nudSection4.Value + nudSection5.Value + nudSection6.Value + nudSection7.Value));
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings7;
                        break;
                    }

                case 8:
                    {
                        nudSection1.Enabled = true; nudSection1.Visible = true;
                        nudSection2.Enabled = true; nudSection2.Visible = true;
                        nudSection3.Enabled = true; nudSection3.Visible = true;
                        nudSection4.Enabled = true; nudSection4.Visible = true;
                        nudSection5.Enabled = true; nudSection5.Visible = true;
                        nudSection6.Enabled = true; nudSection6.Visible = true;
                        nudSection7.Enabled = true; nudSection7.Visible = true;
                        nudSection8.Enabled = true; nudSection8.Visible = true;
                        nudSection1.Left = 10;
                        nudSection2.Left = 133;
                        nudSection3.Left = 254;
                        nudSection4.Left = 372;
                        nudSection5.Left = 489;
                        nudSection6.Left = 607;
                        nudSection7.Left = 724;
                        nudSection8.Left = 839;

                        lblVehicleToolWidth.Text = Convert.ToString((int)(nudSection1.Value + nudSection2.Value + nudSection3.Value +
                            nudSection4.Value + nudSection5.Value + nudSection6.Value + nudSection7.Value + nudSection8.Value));
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings8;
                        break;
                    }
            }
            //update in settings dialog ONLY total tool width
            SectionFeetInchesTotalWidthLabelUpdate();
        }

        //the minimum speed before sections turn off
        private void nudCutoffSpeed_ValueChanged(object sender, EventArgs e)
        {
            cutoffSpeed = (double)nudCutoffSpeed.Value;
        }

        //update tool width label at bottom of window
        private void SectionFeetInchesTotalWidthLabelUpdate()
        {
            if (mf.isMetric)
            {
                lblSecTotalWidthMeters.Text = Convert.ToDouble(lblVehicleToolWidth.Text) + " m";
            }

            else
            {
                double toFeet = (Convert.ToDouble(lblVehicleToolWidth.Text) * 0.08334);
                lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
                double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
                lblSecTotalWidthInches.Text = Convert.ToString(temp) + '"';

                lblSecTotalWidthMeters.Text = (Math.Round((glm.in2m * toFeet * 12.0), 2)).ToString() + " m";
            }
        }

        //Convert section width to positions along toolbar
        private void CalculateSectionPositions()
        {
            int i = (int)nudNumberOfSections.Value;

            //convert to meters spinner value
            sectionWidth1 = nudSection1.Value * (decimal)metImp2m;
            sectionWidth2 = nudSection2.Value * (decimal)metImp2m;
            sectionWidth3 = nudSection3.Value * (decimal)metImp2m;
            sectionWidth4 = nudSection4.Value * (decimal)metImp2m;
            sectionWidth5 = nudSection5.Value * (decimal)metImp2m;
            sectionWidth6 = nudSection6.Value * (decimal)metImp2m;
            sectionWidth7 = nudSection7.Value * (decimal)metImp2m;
            sectionWidth8 = nudSection8.Value * (decimal)metImp2m;

            switch (i)
            {
                case 1:
                    {
                        sectionPosition2 = sectionWidth1 / 2.0M;
                        sectionPosition1 = sectionPosition2 * -1;
                        sectionPosition3 = 0;
                        sectionPosition4 = 0;
                        sectionPosition5 = 0;
                        sectionPosition6 = 0;
                        sectionPosition7 = 0;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        break;
                    }

                case 2:
                    {
                        sectionPosition1 = sectionWidth1 * -1;
                        sectionPosition2 = 0;
                        sectionPosition3 = sectionWidth2;
                        sectionPosition4 = 0;
                        sectionPosition5 = 0;
                        sectionPosition6 = 0;
                        sectionPosition7 = 0;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        break;
                    }

                case 3:
                    {
                        sectionPosition3 = sectionWidth2 / 2.0M;
                        sectionPosition2 = sectionPosition3 * -1;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition4 = sectionPosition3 + sectionWidth3;
                        sectionPosition5 = 0;
                        sectionPosition6 = 0;
                        sectionPosition7 = 0;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        break;
                    }

                case 4:
                    {
                        sectionPosition2 = sectionWidth2 * -1;
                        sectionPosition3 = 0;
                        sectionPosition4 = sectionWidth3;
                        sectionPosition5 = sectionWidth3 + sectionWidth4;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition6 = 0;
                        sectionPosition7 = 0;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        break;
                    }

                case 5:
                    {
                        sectionPosition4 = sectionWidth3 / 2.0M;
                        sectionPosition3 = sectionPosition4 * -1;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition5 = sectionPosition4 + sectionWidth4;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition6 = sectionPosition5 + sectionWidth5;
                        sectionPosition7 = 0;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        break;
                    }
                case 6:
                    {
                        sectionPosition4 = 0;
                        sectionPosition3 = sectionWidth3 * -1;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition5 = sectionWidth4;
                        sectionPosition6 = sectionPosition5 + sectionWidth5;
                        sectionPosition7 = sectionPosition6 + sectionWidth6;
                        sectionPosition8 = 0;
                        sectionPosition9 = 0;
                        break;
                    }

                case 7:
                    {
                        sectionPosition5 = sectionWidth3 / 2.0M;
                        sectionPosition4 = sectionPosition5 * -1;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition6 = sectionPosition5 + sectionWidth5;
                        sectionPosition7 = sectionPosition6 + sectionWidth6;
                        sectionPosition8 = sectionPosition7 + sectionWidth7;
                        sectionPosition9 = 0;
                        break;
                    }

                case 8:
                    {
                        sectionPosition4 = sectionWidth4 * -1;
                        sectionPosition3 = sectionPosition4 - sectionWidth3;
                        sectionPosition2 = sectionPosition3 - sectionWidth2;
                        sectionPosition1 = sectionPosition2 - sectionWidth1;
                        sectionPosition5 = 0;
                        sectionPosition6 = sectionWidth5;
                        sectionPosition7 = sectionPosition6 + sectionWidth6;
                        sectionPosition8 = sectionPosition7 + sectionWidth7;
                        sectionPosition9 = sectionPosition8 + sectionWidth8;
                        break;
                    }


            }
        }

        //Every time the # of Sections is spun
        private void nudNumberOfSections_ValueChanged(object sender, EventArgs e)
        {
            numberOfSections = (int)nudNumberOfSections.Value;

            nudSection1.ValueChanged -= nudSection1_ValueChanged;
            nudSection1.Value = (decimal)(100);
            nudSection1.ValueChanged += nudSection1_ValueChanged;

            nudSection2.ValueChanged -= nudSection2_ValueChanged;
            nudSection2.Value = (decimal)(100);
            nudSection2.ValueChanged += nudSection2_ValueChanged;

            nudSection3.ValueChanged -= nudSection3_ValueChanged;
            nudSection3.Value = (decimal)(100);
            nudSection3.ValueChanged += nudSection3_ValueChanged;

            nudSection4.ValueChanged -= nudSection4_ValueChanged;
            nudSection4.Value = (decimal)(100);
            nudSection4.ValueChanged += nudSection4_ValueChanged;

            nudSection5.ValueChanged -= nudSection5_ValueChanged;
            nudSection5.Value = (decimal)(100);
            nudSection5.ValueChanged += nudSection5_ValueChanged;

            nudSection6.ValueChanged -= nudSection6_ValueChanged;
            nudSection6.Value = (decimal)(100);
            nudSection6.ValueChanged += nudSection6_ValueChanged;

            nudSection7.ValueChanged -= nudSection7_ValueChanged;
            nudSection7.Value = (decimal)(100);
            nudSection7.ValueChanged += nudSection7_ValueChanged;

            nudSection8.ValueChanged -= nudSection8_ValueChanged;
            nudSection8.Value = (decimal)(100);
            nudSection8.ValueChanged += nudSection8_ValueChanged;

            UpdateSpinners();
        }

        //Did user spin a section distance spinner?
        private void nudSection1_ValueChanged(object sender, EventArgs e)
        { UpdateSpinners(); 
            if (Convert.ToDouble(lblVehicleToolWidth.Text) > maxWidth) nudSection1.Value -= 1; }

        private void nudSection2_ValueChanged(object sender, EventArgs e)
        { UpdateSpinners();
            if (Convert.ToDouble(lblVehicleToolWidth.Text) > maxWidth) nudSection2.Value -= 1; }

        private void nudSection3_ValueChanged(object sender, EventArgs e)
        { UpdateSpinners(); 
            if (Convert.ToDouble(lblVehicleToolWidth.Text) > maxWidth) nudSection3.Value -= 1; }

        private void nudSection4_ValueChanged(object sender, EventArgs e)
        { UpdateSpinners(); 
            if (Convert.ToDouble(lblVehicleToolWidth.Text) > maxWidth) nudSection4.Value -= 1; }

        private void nudSection5_ValueChanged(object sender, EventArgs e)
        { UpdateSpinners(); 
            if (Convert.ToDouble(lblVehicleToolWidth.Text) > maxWidth) nudSection5.Value -= 1; }
 
       private void nudSection6_ValueChanged(object sender, EventArgs e)
        { UpdateSpinners();
            if (Convert.ToDouble(lblVehicleToolWidth.Text) > maxWidth) nudSection6.Value -= 1; }

        private void nudSection7_ValueChanged(object sender, EventArgs e)
       { UpdateSpinners();
           if (Convert.ToDouble(lblVehicleToolWidth.Text) > maxWidth) nudSection7.Value -= 1;  }

        private void nudSection8_ValueChanged(object sender, EventArgs e)
        { UpdateSpinners();
           if (Convert.ToDouble(lblVehicleToolWidth.Text) > maxWidth) nudSection8.Value -= 1;  }
        
        #endregion Sections

        #region Guidance //----------------------------------------------------------------




#endregion Guidance

        #region Display //----------------------------------------------------------------
        
        private void nudTriangleResolution_ValueChanged(object sender, EventArgs e)
        {
            triResolution = nudTriangleResolution.Value;
        }

        private void nudMinFixStepDistance_ValueChanged(object sender, EventArgs e)
        {
            minFixStepDistance = nudMinFixStepDistance.Value;
        }

        private void chkIsAtanCam_CheckedChanged(object sender, EventArgs e)
        {
            isAtanCamera = !isAtanCamera;
            chkIsAtanCam.Checked = isAtanCamera;
        }

        #endregion

        #region WorkSwitch //---------------------------------------------------------

        private void chkWorkSwActiveLow_CheckedChanged(object sender, EventArgs e)
        {
            isWorkSwActiveLow = !isWorkSwActiveLow;
            chkWorkSwActiveLow.Checked = isWorkSwActiveLow;
        }

        private void chkEnableWorkSwitch_CheckedChanged(object sender, EventArgs e)
        {
            isWorkSwEn = !isWorkSwEn;
            chkEnableWorkSwitch.Checked = isWorkSwEn;
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblPitchFromIMU.Text = (mf.modcom.pitchAngle).ToString();
            lblRollFromIMU.Text = (mf.modcom.rollAngle).ToString();
            lblRollOffset.Text = rollZeroSet.ToString();
            lblPitchOffset.Text = pitchZeroSet.ToString();
        }

        private void btnRollPitchZero_Click(object sender, EventArgs e)
        {
            pitchZeroSet = -mf.modcom.pitchAngle;
            rollZeroSet = -mf.modcom.rollAngle;
        }




 

    }
}


/*
        private void CalculateSectionSpinners()
        {
            int i = numberOfSections;

            nudSection1.ValueChanged -= nudSection1_ValueChanged;
            nudSection2.ValueChanged -= nudSection2_ValueChanged;
            nudSection3.ValueChanged -= nudSection3_ValueChanged;
            nudSection4.ValueChanged -= nudSection4_ValueChanged;
            nudSection5.ValueChanged -= nudSection5_ValueChanged;


            switch (i)
            {


                case 1:
                    {
                        nudSection2.Value = (nudCalc.Value * 0.5M);
                        nudSection1.Value = (-nudCalc.Value * 0.5M);
                        spinSection1 = (double)(-nudCalc.Value * 0.5M * 0.0254M);
                        spinSection2 = (double)(nudCalc.Value * 0.5M * 0.0254M);
                        lblVehicleToolWidth.Text = Convert.ToString(Math.Round(nudSection2.Value - nudSection1.Value,2));
                        nudSection1.Left = 31;
                        nudSection2.Left = 816;
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings1;
                        break;
                    }
                case 2:
                    {
                        nudSection1.Value = (-nudCalc.Value * 0.5M);
                        nudSection2.Value = 0;
                        nudSection3.Value = (nudCalc.Value * 0.5M);
                        spinSection1 = (double)(-nudCalc.Value * 0.5M * 0.0254M);
                        spinSection2 = 0;
                        spinSection3 = (double)(nudCalc.Value * 0.5M * 0.0254M);

                        lblVehicleToolWidth.Text = Convert.ToString(Math.Round(nudSection3.Value - nudSection1.Value,2));
                        nudSection1.Left = 31;
                        nudSection2.Left = 434;
                        nudSection3.Left = 816;
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings2;

                        break;
                    }

                case 3:
                    {
                        nudSection1.Value = (-nudCalc.Value * 0.5M);
                        nudSection2.Value = (-nudCalc.Value * 0.16M);
                        nudSection3.Value = (nudCalc.Value * 0.16M);
                        nudSection4.Value = (nudCalc.Value * 0.5M);
                        spinSection1 = (double)(-nudCalc.Value * 0.5M * 0.0254M);
                        spinSection2 = (double)(-nudCalc.Value * 0.16M * 0.0254M);
                        spinSection3 = (double)(nudCalc.Value * 0.16M * 0.0254M);
                        spinSection4 = (double)(nudCalc.Value * 0.5M * 0.0254M);
                        lblVehicleToolWidth.Text = Convert.ToString(Math.Round(nudSection4.Value - nudSection1.Value,2));
                        nudSection1.Left = 31;
                        nudSection2.Left = 291;
                        nudSection3.Left = 567;
                        nudSection4.Left = 816;
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings3;
                        break;
                    }

                case 4:
                    {
                        nudSection1.Value = (-nudCalc.Value * 0.5M);
                        nudSection2.Value = (-nudCalc.Value * 0.25M);
                        nudSection3.Value = 0;
                        nudSection4.Value = (nudCalc.Value * 0.25M);
                        nudSection5.Value = (nudCalc.Value * 0.5M);
                        spinSection1 = (double)(-nudCalc.Value * 0.5M * 0.0254M);
                        spinSection2 = (double)(-nudCalc.Value * 0.25M * 0.0254M);
                        spinSection3 = 0;
                        spinSection4 = (double)(nudCalc.Value * 0.25M * 0.0254M);
                        spinSection5 = (double)(nudCalc.Value * 0.5M * 0.0254M);

                        lblVehicleToolWidth.Text = Convert.ToString(Math.Round(nudSection5.Value - nudSection1.Value, 2));
                        nudSection1.Left = 31;
                        nudSection2.Left = 226;
                        nudSection3.Left = 434;
                        nudSection4.Left = 660;
                        nudSection5.Left = 816;
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings4;
                        break;
                    }

                case 5:
                    {
                        nudSection1.Value = (-nudCalc.Value * 0.5M);
                        nudSection2.Value = (-nudCalc.Value * 0.3M);
                        nudSection3.Value = (-nudCalc.Value * 0.1M);
                        nudSection4.Value = (nudCalc.Value * 0.1M);
                        nudSection5.Value = (nudCalc.Value * 0.3M);
                        spinSection1 = (double)(-nudCalc.Value * 0.5M * 0.0254M); ;
                        spinSection2 = (double)(-nudCalc.Value * 0.3M * 0.0254M);
                        spinSection3 = (double)(-nudCalc.Value * 0.1M * 0.0254M);
                        spinSection4 = (double)(nudCalc.Value * 0.1M * 0.0254M);
                        spinSection5 = (double)(nudCalc.Value * 0.3M * 0.0254M);

                        //lblVehicleToolWidth.Text = Convert.ToString(Math.Round(nudSection6.Value - nudSection1.Value, 2));
                        nudSection1.Left = 31;
                        nudSection2.Left = 188;
                        nudSection3.Left = 345;
                        nudSection4.Left = 502;
                        nudSection5.Left = 660;
                        tabSections.BackgroundImage = global::AgOpenGPS.Properties.Resources.SectionSettings5;
                        break;
                    }
            }

            SectionFeetInchesTotalWidthLabelUpdate();
            UpdateSpinners();

            nudSection1.ValueChanged += nudSection1_ValueChanged;
            nudSection2.ValueChanged += nudSection2_ValueChanged;
            nudSection3.ValueChanged += nudSection3_ValueChanged;
            nudSection4.ValueChanged += nudSection4_ValueChanged;
            nudSection5.ValueChanged += nudSection5_ValueChanged;
        }
        */