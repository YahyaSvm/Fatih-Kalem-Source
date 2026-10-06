; Fatih Kalem - Windows kurulum sihirbazı (Inno Setup 6)
; Derleme: iscc /DAppVersion=2.1.0 /DSourceDir=..\dist\windows-build installer\fatih-kalem.iss

#ifndef AppVersion
  #define AppVersion "2.1.0"
#endif
#ifndef SourceDir
  #define SourceDir "..\dist\windows-build"
#endif

#define AppName "Fatih Kalem"
#define AppExe "Fatih Kalem.exe"
#define AppUrl "https://github.com/YahyaSvm/Fatih-Kalem-Source"

[Setup]
AppId={{6B3F2C1E-5D7A-4E8B-9C21-7A4F0D9E2B55}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher=Yahya Eren Sevim (YhySvm)
AppPublisherURL={#AppUrl}
AppSupportURL={#AppUrl}/issues
AppUpdatesURL={#AppUrl}/releases
DefaultDirName={autopf}\Fatih Kalem
DefaultGroupName=Fatih Kalem
DisableProgramGroupPage=yes
; Öğretmen hesaplarında yönetici yetkisi olmayabilir: varsayılan kullanıcıya kur,
; isteyen "tüm kullanıcılar" için yönetici olarak kurabilir.
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=..\dist\windows
OutputBaseFilename=FatihKalem-{#AppVersion}-kurulum
SetupIconFile=..\app.ico
UninstallDisplayIcon={app}\{#AppExe}
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
CloseApplications=yes
RestartApplications=no
MinVersion=6.1sp1

[Languages]
Name: "turkish"; MessagesFile: "compiler:Languages\Turkish.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[CustomMessages]
turkish.AutoStart=Windows açılışında otomatik başlat
english.AutoStart=Start automatically when Windows starts
turkish.Extra=Ek seçenekler:
english.Extra=Additional options:

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "autostart"; Description: "{cm:AutoStart}"; GroupDescription: "{cm:Extra}"; Flags: unchecked

[Files]
Source: "{#SourceDir}\{#AppExe}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\{#AppExe}.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\README.md"; DestDir: "{app}"; DestName: "BENIOKU.md"; Flags: ignoreversion
Source: "..\LICENSE"; DestDir: "{app}"; DestName: "LICENSE.txt"; Flags: ignoreversion

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExe}"
Name: "{group}\{cm:UninstallProgram,{#AppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExe}"; Tasks: desktopicon

[Registry]
; Programın kendi "Otomatik başlat" ayarıyla aynı kayıt (tırnaklı yol)
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "Fatih Pen"; ValueData: """{app}\{#AppExe}"""; Tasks: autostart; Flags: uninsdeletevalue

[Run]
Filename: "{app}\{#AppExe}"; Description: "{cm:LaunchProgram,{#AppName}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{localappdata}\Fatih Kalem"
