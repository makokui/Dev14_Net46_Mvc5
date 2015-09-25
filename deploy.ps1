
# ----------------------
# KUDU Deployment Script
# Version: 1.0.0
# ----------------------

# Helpers
# -------

function exitWithMessageOnError($1) {
  if ($? -eq $false) {
    echo "An error has occurred during web site deployment."
    echo $1
    exit 1
  }
}

# Prerequisites
# -------------

# Verify node.js installed
where.exe node 2> $null > $null
exitWithMessageOnError "Missing node.js executable, please install node.js, if already installed make sure it can be reached from current environment."

# Setup
# -----

$SCRIPT_DIR = $PSScriptRoot
$ARTIFACTS = "$SCRIPT_DIR\..\artifacts"

$KUDU_SYNC_CMD = $env:KUDU_SYNC_CMD

$DEPLOYMENT_SOURCE = $env:DEPLOYMENT_SOURCE
$DEPLOYMENT_TARGET = $env:DEPLOYMENT_TARGET

$NEXT_MANIFEST_PATH = $env:NEXT_MANIFEST_PATH
$PREVIOUS_MANIFEST_PATH = $env:PREVIOUS_MANIFEST_PATH

if ($DEPLOYMENT_SOURCE -eq $null) {
  $DEPLOYMENT_SOURCE = $SCRIPT_DIR
}

if ($DEPLOYMENT_TARGET -eq $null) {
  $DEPLOYMENT_TARGET = "$ARTIFACTS\wwwroot"
}

if ($NEXT_MANIFEST_PATH -eq $null) {
  $NEXT_MANIFEST_PATH = "$ARTIFACTS\manifest"

  if ($PREVIOUS_MANIFEST_PATH -eq $null) {
    $PREVIOUS_MANIFEST_PATH = $NEXT_MANIFEST_PATH
  }
}

if ($KUDU_SYNC_CMD -eq $null) {
  # Install kudu sync
  echo "Installing Kudu Sync"
  npm install kudusync -g --silent
  exitWithMessageOnError "npm failed"

  # Locally just running "kuduSync" would also work
  $KUDU_SYNC_CMD = "$env:APPDATA\npm\kuduSync.cmd"
}

$DEPLOYMENT_TEMP = $env:DEPLOYMENT_TEMP
$MSBUILD_PATH = $env:MSBUILD_PATH

if ($DEPLOYMENT_TEMP -eq $null) {
  $DEPLOYMENT_TEMP = "$env:temp\___deployTemp$env:random"
  $CLEAN_LOCAL_DEPLOYMENT_TEMP = $true
}

if ($CLEAN_LOCAL_DEPLOYMENT_TEMP -eq $true) {
  if (Test-Path $DEPLOYMENT_TEMP) {
    rd -Path $DEPLOYMENT_TEMP -Recurse -Force
  }
  mkdir "$DEPLOYMENT_TEMP"
}

if ($MSBUILD_PATH -eq $null) {
  $MSBUILD_PATH = "$env:ProgramFiles\MSBuild\14.0\Bin\MSBuild.exe"
}

##################################################################################################################################
# Deployment
# ----------

echo "Handling .NET Web Application deployment."

# 1. Restore NuGet packages
if (Test-Path "Dev14_Net46_Mvc5.sln") {
  nuget restore "$DEPLOYMENT_SOURCE\Dev14_Net46_Mvc5.sln"
  exitWithMessageOnError "NuGet restore failed"
}

# 2. Build to the temporary path
if ($env:IN_PLACE_DEPLOYMENT -ne "1") {
  & $MSBUILD_PATH "$DEPLOYMENT_SOURCE\Dev14_Net46_Mvc5\Dev14_Net46_Mvc5.csproj" /nologo /verbosity:m /t:Build /t:pipelinePreDeployCopyAllFilesToOneFolder /p:_PackageTempDir="$DEPLOYMENT_TEMP"`;AutoParameterizationWebConfigConnectionStrings=false`;Configuration=Release /p:SolutionDir="$DEPLOYMENT_SOURCE\.\\" $env:SCM_BUILD_ARGS
} else {
  & $MSBUILD_PATH "$DEPLOYMENT_SOURCE\Dev14_Net46_Mvc5\Dev14_Net46_Mvc5.csproj" /nologo /verbosity:m /t:Build /p:AutoParameterizationWebConfigConnectionStrings=false`;Configuration=Release /p:SolutionDir="$DEPLOYMENT_SOURCE\.\\" $env:SCM_BUILD_ARGS
}

exitWithMessageOnError "MSBuild failed"

# 3. KuduSync
if ($env:IN_PLACE_DEPLOYMENT -ne "1") {
  & $KUDU_SYNC_CMD -v 50 -f "$DEPLOYMENT_TEMP" -t "$DEPLOYMENT_TARGET" -n "$NEXT_MANIFEST_PATH" -p "$PREVIOUS_MANIFEST_PATH" -i ".git;.hg;.deployment;deploy.ps1"
  exitWithMessageOnError "Kudu Sync failed"
}

##################################################################################################################################

# Post deployment stub
if ($env:POST_DEPLOYMENT_ACTION -ne $null) {
  & $env:POST_DEPLOYMENT_ACTION
  exitWithMessageOnError "post deployment action failed"
}

echo "Finished successfully."
