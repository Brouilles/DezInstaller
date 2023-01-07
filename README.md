# DezInstaller
WPF interface for a WiX toolset installer.
It is an example, a template, not a tool.

You need to have WiX Toolset setup https://wixtoolset.org/.

## How to use?

### Step 1
Copy your application executables and DLL in `SetupProject2/items`.

### Step 2
Open the Bootstrapper project with rigth click, properties. You can change the name of the final exe in the **Output name** input.
Also, open the **Localization.wxl** file and change the *Manufacturer* and *BundleName* values according your app.

### Step 3
In the **InstallerUI** project, you can change the *banner.png* and *LICENSE.txt* in the Assets folder.
Also, you can customize like you want the views and pages of the installers, it is a normal WPF app.
Remember to change in Properties, *Resource.resx* file *APP_EXE* and *APP_NAME* strings.

### Step 4
Now, remember to switch to Release.

* Build **SetupProject2** (two times if you have an error, to regenerate the list of the files of your app)
* Build **InstallerUI**
* Build **Bootstrapper**

Your final exe is located at `GameInstaller/Bootstrapper/bin/Release`.