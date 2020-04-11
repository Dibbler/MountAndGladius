$destination = "C:\Program Files\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\MountAndGladiusII\"
$binaryDir = ($destination + "bin\Win64_Shipping_Client")
$xmlDir = ($destination + "ModuleData")

MD $destination -Force
MD $binaryDir -Force
MD $xmlDir -Force

#Copy built files
Copy-Item "*" -Include "*.dll", "*.pdb" -Destination $binaryDir -Recurse -Force

#Copy SubModule config
Copy-Item -Path "..\..\SubModule.xml" -Destination $destination -Force

#Copy ModuleData
Copy-Item -Path "..\..\ModuleData\*" -Include "*.xml" -Destination $xmlDir -Force