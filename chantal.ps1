
Add-Type -AssemblyName System.Windows.Forms

$global:filename1 = ""

#Load assembly that allows an inputbox
[void][Reflection.Assembly]::LoadWithPartialName('Microsoft.VisualBasic')
$regkey1 = ""
try
{
    $regkey1 = Get-ItemProperty -Path HKCU:\SOFTWARE\TSAutomator -ErrorAction Stop | Select-Object -ExpandProperty Location
}catch
{
    $value1 = [Microsoft.VisualBasic.Interaction]::InputBox("Please enter the folder path of the dll files (note: please put files on C drive)?", "Path to dll files", "$env:USERPROFILE\Desktop\TSAutomator")
    if($value1 -ne "")
    {
        $item1 = New-Item -Path HKCU:\SOFTWARE -Name TSAutomator -Force
        $item2 = New-ItemProperty -Path 'HKCU:Software\TSAutomator' -Name 'Location' -Value $value1
        $regkey1 = $value1
    }
}

try
{
    #Add UI automation tools modules that allow RPA between windoes
    Add-Type -path "$regkey1\\UIAutomationClient.dll"
    Add-Type -path "$regkey1\\UIAutomationTypes.dll"
}catch
{
    $value1 = [Microsoft.VisualBasic.Interaction]::InputBox("Please enter the CORRECT folder path of the dll files (note: please put files on C drive)?", "Path to dll files", "$env:USERPROFILE\Desktop\TSAutomator")
    if($value1 -ne "")
    {
        try
        {
            Set-Item -Path HKCU:\SOFTWARE\TSAutomator\Location -Value $value1 -ErrorAction Stop
            #Add UI automation tools modules that allow RPA between windoes
        }catch
        {
            $item2 = New-ItemProperty -Path 'HKCU:Software\TSAutomator' -Name 'Location' -Value $value1
        }
        Add-Type -path "$value1\\UIAutomationClient.dll"
        Add-Type -path "$value1\\UIAutomationTypes.dll"
    }
}



#This function opens an incident in servicenow
function ProcessSpreadsheet($file1, $address1)
{
    
    
    #minimize the main window
    $updatemonitorassets.WindowState = [System.Windows.Forms.FormWindowState]::Minimized
    
    $snproc = [System.Diagnostics.Process]::start("microsoft-edge:https://canadalife.service-now.com/")

    #verify that edge has opened correctly
    $processes = [System.Diagnostics.Process]::GetProcessesByName("msedge")
    if($processes.Length -eq 0)
    {
    }else
    {
        #Get currently opened edge window
        foreach($proc1 in $processes)
        {
            if($proc1.MainWindowHandle -ne [System.IntPtr]::Zero)
            {
                $snproc = $proc1
            }
        }
    }

        #initialize processes that will be automated and start ServiceNow in edge browser
    
    #allow time for ServiceNow to open
    Start-Sleep 3
    
    
    #get the main automationelement 
    $rootElement = [System.Windows.Automation.AutomationElement]::FromHandle($snproc.MainWindowHandle)

    #set the edge/servicenow window as active
    $rootElement.SetFocus

    #set main automation element variables
    $classnameidentifier1 = [System.Windows.Automation.AutomationElement]::ClassNameProperty
    $treescope1 = [System.Windows.Automation.TreeScope]::Descendants

    #set the filter icon variable
    $filtercondition1 = [System.Windows.Automation.PropertyCondition]::new($classnameidentifier1, "breadcrumb_link")

    #set the variable that will look for the all applications tab when active
    $checkallselected1 = [System.Windows.Automation.PropertyCondition]::new($classnameidentifier1, "btn btn-icon sn-navhub-btn sn-tooltip-basic ng-scope allApps icon-all-apps state-active")

    #set the variable that will look for the all applications tab when not active
    $checkallselected2 = [System.Windows.Automation.PropertyCondition]::new($classnameidentifier1, "btn btn-icon sn-navhub-btn sn-tooltip-basic ng-scope allApps icon-all-apps")

    #set the variable that will look for if the incidents option is not expanded
    #$checkcatopen1 = [System.Windows.Automation.PropertyCondition]::new($classnameidentifier1, "app-node sn-aside-group-title sn-aside-group-title_selectable sn-aside-group-title_nav sn-aside-group-title_hidden nav-application-overwrite collapsed")

    $allcondition1 = [System.Windows.Automation.PropertyCondition]::TrueCondition

    #set the variable that will look for the Incidents button option
    $condition1 = [System.Windows.Automation.PropertyCondition]::new($classnameidentifier1, "sn-widget-list-item sn-widget-list-item_hidden-action module-node")

    #set the variable that will look for the All link to search without filters
    $condition2 = [System.Windows.Automation.PropertyCondition]::new($classnameidentifier1, "breadcrumb_link")
    
    $condition3 = [System.Windows.Automation.PropertyCondition]::new($classnameidentifier1, "linked formlink")

    #make sure window is active
    if($rootElement -eq $null)
    {
        #Send an error message indicating the action failed
        Read-MessageBoxDialog "Problem connecting to edge due to an error. Please restart this application and the browser." "Action failed"
    }else
    {
        #
        $counttries1 = 0
        $invoked1 = $true
        while($counttries1 -lt 8 -and $invoked1 -eq $true)
        {
            if($rootElement.FindAll($treescope1, $checkallselected1).Length -gt 0)
            {
                $invoked1 = $false
            }
            #search for automation element = All applications list of searchable items
            foreach($child in $rootElement.FindAll($treescope1, $checkallselected2))
            {
                #check if it is the All applications tab child element
                if ($child.Current.Name -like "*All applications")
                {
                    #get child's pattern to be able to do actions on the automation element
                    $pattern1 = [System.Windows.Automation.SelectionItemPatternIdentifiers]::Pattern 
                    $etb = $child.GetCurrentPattern($pattern1)

                    #set the automation element/control as the active control
                    $child.SetFocus()

                    #Send a click/activate onto the control
                    #$etb.Invoke()                    
                    $etb.select()
                    $invoked1 = $false
                }
            }
            #search for automation element = All applications list of searchable items that is already active
            foreach($child in $rootElement.FindAll($treescope1, $checkallselected1))
            {
                #check if it is the All applications tab child element
                if ($child.Current.Name -like "*All applications")
                {                  
                    $invoked1 = $false
                }
            }
            if($invoked1 -eq $true -and $counttries1 -eq 4)
            {
                $global:proc = [System.Diagnostics.Process]::start("microsoft-edge:https://canadalife.service-now.com/")
                $global:processes = [System.Diagnostics.Process]::GetProcessesByName("msedge")

                #Wait for ServiceNow page
                Start-Sleep 3

                if($global:processes.Length -eq 0)
                {
                }else
                {
                    foreach($global:proc1 in $global:processes)
                    {
                        if($global:proc1.MainWindowHandle -ne [System.IntPtr]::Zero)
                        {
                            $global:proc = $global:proc1
                        }
                    }
                }
                
                if($global:proc.MainWindowHandle -ne $null)
                {
                    #get the main automationelement 
                    $rootElement = [System.Windows.Automation.AutomationElement]::FromHandle($global:proc.MainWindowHandle)

                    #set the edge/servicenow window as active
                    $rootElement.SetFocus
                }else
                {
                    #Send an error message indicating the action failed
                    Read-MessageBoxDialog "Problem getting control of edge window. Please restart this application and the browser." "Action failed"
                    $counttries1 = 2
                }
                
            }

            #Wait for ServiceNow page
            Start-Sleep -Milliseconds 500
            $counttries1 = $counttries1 + 1
        }

        
        #set flag variables to use during loop
        $invoked1 = $true
        $counttries1 = 0


        #loop until time is elapsed or action is taken
        while($counttries1 -lt 5 -and $invoked1 -eq $true)
        {
            #search for automation element = "Incident" to verify if it is expanded
            foreach($child in $rootElement.FindAll($treescope1, $allcondition1))
            {
                try
                {
                    #check if it is the incident dropdown child element
                    if ($child.Current.Name -Contains "Asset")
                    {
                        #get child's pattern to be able to do actions on the automation element
                        $pattern1 = [System.Windows.Automation.ExpandCollapsePattern]::Pattern 
                        $etb = $child.GetCurrentPattern($pattern1)

                        #set the automation element/control as the active control
                        $child.SetFocus()

                        #expand the dropdown list to make the options available
                        $etb.Expand()

                        $invoked1 = $false
                    }
                }catch
                {
                }
            }
            $counttries1 = $counttries1 + 1
        }

        $barcode1 = ""
        $location1 = ""
        $seat1 = ""
        $rownumber1 = 2

        $invoked1 = $true
        $counttries2 = 0


        #get the main automationelement 
        $conditionsearch1 = [System.Windows.Automation.PropertyCondition]::new($classnameidentifier1, "form-control")

        
            #start the reader component and open the file
            $reader1 = [System.IO.StreamReader]::new($file1)

            #split the line into 4 values
                $line1 = $reader1.ReadLine().Split(",")

        #loop until time is elapsed or action is taken
        Do
        {
            


            #read until the end of the file
            if($reader1.EndOfStream -eq $false)
            {
                #split the line into 4 values
                $line1 = $reader1.ReadLine().Split(",")

                #confirm that line split correctly
                if($line1[0] -ne "" -and $line1[0] -ne $null -and $line1.Count -ne 0)
                {
                    #add the data into variables and listbox
                    $barcode1 = $line1[0].Trim("""")
                    $location1 = $line1[1].Trim("""")
                    $seat1 = $line1[2].Trim("""")
                }
            
            }else
            {
                $barcode1 = ""
            }
            if($barcode1 -ne "")
            {
                #search for automation element = "Incidents" to verify if it is expanded
                foreach($child in $rootElement.FindAll($treescope1, $condition1))
                {
                    #check if it is the incident dropdown child element
                    if ($child.Current.Name -Contains "Monitor")
                    {
                        #get child's pattern to be able to do actions on the automation element
                        $pattern1 = [System.Windows.Automation.InvokePattern]::Pattern
                        $etb = $child.GetCurrentPattern($pattern1)

                        #set the automation element/control as the active control
                        $child.SetFocus()

                        #expand the dropdown list to make the options available
                        $etb.invoke()


                        #stop the loop
                        $invoked1 = $false
                    }
                }

                Start-Sleep 2

                #search for automation element = "Incidents" to verify if it is expanded
                foreach($child in $rootElement.FindAll($treescope1, $conditionsearch1))
                {
                    #check if it is the incident dropdown child element
                    if ($child.Current.Name -Contains "Search")
                    {
                        #get child's pattern to be able to do actions on the automation element
                        $pattern1 = [System.Windows.Automation.ValuePattern]::Pattern
                        $etb = $child.GetCurrentPattern($pattern1)

                        #set the automation element/control as the active control
                        $child.SetFocus()

                        #expand the dropdown list to make the options available
                        $etb.setvalue("$barcode1")

                        Start-Sleep -Milliseconds 500
                        #type the body message
                        [System.Windows.Forms.SendKeys]::SendWait("{ENTER}")

                        #stop the loop
                        $invoked1 = $false
                    }
                }

            
                #Wait for ServiceNow page
                Start-Sleep -Milliseconds 1500
                #search for automation element = "Incident" to verify if it is expanded
                foreach($child in $rootElement.FindAll($treescope1, $allcondition1))
                {
                    try
                    {
                        #check if it is the incident dropdown child element
                        if ($child.Current.Name -like "*$barcode1 - Open record: *")
                        {
                            #get child's pattern to be able to do actions on the automation element
                            $pattern1 = [System.Windows.Automation.InvokePattern]::Pattern 
                            $etb = $child.GetCurrentPattern($pattern1)


                            #expand the dropdown list to make the options available
                            $etb.Invoke()

                            Continue
                        }
                    }catch
                    {
                    }
                    
                }
            }

            Start-Sleep -Milliseconds 1000
            $invoked1 = $true
            $counttries1 = 0
            $conditionboxes = [System.Windows.Automation.PropertyCondition]::new($classnameidentifier1, "form-control element_reference_input")
            $conditioncomments = [System.Windows.Automation.PropertyCondition]::new($classnameidentifier1, "form-control")

            $check1 = $false
            while($counttries1 -lt 5 -and $invoked1 -eq $true)
            {
                #search for automation element = All incidents
                foreach($child in $rootElement.FindAll($treescope1, $allcondition1))
                {
                    #check if it is the All link child element
                    if ($child.Current.Name -like "Location")
                    {
                        if($child.Current.Value -eq $address1)
                        {
                            $check1 = $true
                        }

                        Continue

                    }

                }
                #search for automation element = All incidents
                foreach($child in $rootElement.FindAll($treescope1, $allcondition1))
                {
                    if ($child.Current.Name -like "Assigned to" -and $check1 -eq $true)
                    {
                        try
                        {
                            #get child's pattern to be able to do actions on the automation element
                            $pattern1 = [System.Windows.Automation.ValuePattern]::Pattern 
                            $etb = $child.GetCurrentPattern($pattern1)

                            #set the automation element/control as the active control
                            $etb.setvalue("")

                            #set the invoked variable to stop the loop
                            $invoked1 = $false
                            Continue
                        }catch
                        {
                        }
                        
                    }
                }
                #search for automation element = All incidents
                foreach($child in $rootElement.FindAll($treescope1, $conditioncomments))
                {
                    #check if it is the All link child element
                    if ($child.Current.Name -like "Comments")
                    {
                        #get child's pattern to be able to do actions on the automation element
                        $pattern1 = [System.Windows.Automation.ValuePattern]::Pattern 
                        $etb = $child.GetCurrentPattern($pattern1)

                        $etb.setvalue("$location1, $seat1")
                        Continue

                    }

                }
                
                #Wait for ServiceNow page
                Start-Sleep -Milliseconds 500
                $counttries1 = $counttries1 + 1
            }

            foreach($child in $rootElement.FindAll($treescope1, $allcondition1))
            {
                try
                {
                    #check if it is the incident dropdown child element
                    if ($child.Current.Name -like "additional actions menu")
                    {
                        #get child's pattern to be able to do actions on the automation element
                        $pattern1 = [System.Windows.Automation.ExpandCollapsePattern]::Pattern 
                        $etb = $child.GetCurrentPattern($pattern1)


                        #expand the dropdown list to make the options available
                        $etb.expand()

                        Start-Sleep -Milliseconds 500
                        #type the body message
                        [System.Windows.Forms.SendKeys]::SendWait("{ENTER}")

                        Continue
                    }
                }catch
                {
                }
                    
            }
            

            #increase the loop counter
            $counttries2 = $counttries2 + 1
        }while($barcode1 -ne "" -and $counttries2 -lt 4)
        
}
    

    
    #minimize the main window
    $updatemonitorassets.WindowState = [System.Windows.Forms.FormWindowState]::Normal
}


function updatemonitorassets
{
    
    #initialize the form
    $updatemonitorassets = New-Object System.Windows.Forms.Form
    $updatemonitorassets.Size = New-Object System.Drawing.Size(390, 350)  
    $updatemonitorassets.Text = "Update monitor asset info"

    #Listbox to select a location
    $listboxlocations = New-Object System.Windows.Forms.ListBox
    $listboxlocations.FormattingEnabled = $True
    $listboxlocations.Location = New-Object System.Drawing.Point(5, 5)
    $listboxlocations.Name = "listbox"
    $listboxlocations.TabIndex = 1
    $listboxlocations.Size = New-Object System.Drawing.Size(295, 95)

    #Label to get location if correct option is not available to select    
    $otherlabel = New-Object System.Windows.Forms.Label
    $otherlabel.Location = New-Object System.Drawing.Point(5, 110)
    $otherlabel.Text = "Other address :"
    $otherlabel.Size = New-Object System.Drawing.Size(100, 25)
    $updatemonitorassets.Controls.Add($otherlabel)

    #Textbox to get location if correct option is not available to select    
    $other1 = New-Object System.Windows.Forms.TextBox
    $other1.Location = New-Object System.Drawing.Point(110, 110)
    $other1.Name = "otheraddress"
    $other1.Size = New-Object System.Drawing.Size(180, 25)
    $other1.Enabled = $false
    $other1.TabIndex = 2
    $updatemonitorassets.Controls.Add($other1)  
    
    #Add locations to select
    $option1 = $listboxlocations.Items.add("60 Osborne Street North, Winnipeg, Manitoba")
    $option1 = $listboxlocations.Items.add("Other (enter below)")
    $listboxlocations.add_Click(
        {
            #set other address textbox as active if the other address option is selected
            if($listboxlocations.SelectedItem.ToString() -eq "Other (enter below)")
            {
                $other1.Enabled = $true
            }else
            {
                $other1.Enabled = $false
            }
        }
    )
    $updatemonitorassets.Controls.Add($listboxlocations)

    

    #Label to get the barcode to refresh/swap on the new request
    $filelabel = New-Object System.Windows.Forms.Label
    $filelabel.Location = New-Object System.Drawing.Point(5, 140)
    $filelabel.Text = "File location"
    $filelabel.Size = New-Object System.Drawing.Size(100, 25)
    $updatemonitorassets.Controls.Add($filelabel)

    #Textbox to get the barcode to refresh/swap on the new request
    $filelocation1 = New-Object System.Windows.Forms.TextBox
    $filelocation1.Location = New-Object System.Drawing.Point(110, 140)
    $filelocation1.Name = "barcode"
    $filelocation1.TabIndex = 3
    $filelocation1.Size = New-Object System.Drawing.Size(180, 25)
    $updatemonitorassets.Controls.Add($filelocation1)  
    
    #button that starts action in servicenow and closes this form
    $BrowseButton = New-Object System.Windows.Forms.Button
    $BrowseButton.Location = New-Object System.Drawing.Point(290, 140)
    $BrowseButton.Text = "Browse..."
    $BrowseButton.TabIndex = 4
    $BrowseButton.Size = New-Object System.Drawing.Size(75,20)
    $BrowseButton.Add_Click(
        {
                #start the file browser    
            $filediaglog1 = New-Object System.Windows.Forms.OpenFileDialog

            #set type of file to open
            $filediaglog1.Filter = "CSV file (*.csv)|*.csv"

            #set the position to put the filter
            $filediaglog1.FilterIndex = 2

            #keep the folder that 
            $filediaglog1.RestoreDirectory = $true

    
            #if an active file is selected proceed with opening
            if($filediaglog1.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK)
            {
                #get the file name selected
                $filelocation1.Text = $filediaglog1.FileName
            }
        }
    )
    $updatemonitorassets.Controls.Add($BrowseButton)

    #button that starts action in servicenow and closes this form
    $OkButton = New-Object System.Windows.Forms.Button
    $OkButton.Location = New-Object System.Drawing.Point(5, 210)
    $OkButton.Text = "Start"
    $OkButton.TabIndex = 5
    $OkButton.Size = New-Object System.Drawing.Size(100,20)
    $OkButton.Add_Click(
        {
            ProcessSpreadsheet $filelocation1.Text $listboxlocations.SelectedItem.ToString()
            #close this form
        }
    )
    $updatemonitorassets.Controls.Add($OkButton)
    

    #Button that cancels the request to create a refresh/swap request and closes this form
    $CancelButton = New-Object System.Windows.Forms.Button
    $CancelButton.Location = New-Object System.Drawing.Point(175, 210)
    $CancelButton.Text = "Cancel"
    $CancelButton.TabIndex = 6
    $CancelButton.Size = New-Object System.Drawing.Size(100,20)
    $CancelButton.Add_Click(
        {
            $updatemonitorassets.Close()
        }
    )
    $updatemonitorassets.Controls.Add($CancelButton)

    #show this form
    $updatemonitorassets.Add_Shown({$updatemonitorassets.Activate()})
    [void] $updatemonitorassets.ShowDialog()

}

updatemonitorassets