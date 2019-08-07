Set-Location D:/projects/xsd_egais
$sourceFiles = (Get-ChildItem D:/projects/xsd_egais/ -filter "*.xsd")
foreach($file in $sourceFiles) {
    $fileName = $file.BaseName+'.cs'
    $namespace = 'Egais.Entities.'+$file.BaseName
    D:/projects/xsd_egais/Xsd2Code.exe $file $namespace $fileName /pl Net35 /xa /is /dc+ /db /eit    
    #echo $fileName
}