$destination = "C:\Program Files\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\MountAndGladius\"
$binaryDir = ($destination + "\bin\Win64_Shipping_Client")

MD $destination -Force
MD $binaryDir -Force

#Copy built files
Copy-Item "*" -Include "*.dll", "*.pdb" -Destination $binaryDir -Recurse -Force

#Copy SubModule config
Copy-Item -Path "..\..\SubModule.xml" -Destination $destination -Force