# Go to XD project directory
projDir=`dirname "$0"`/..
pushd "$projDir" > /dev/null

# Create docker image
username="maciejdziuban"
image_name=xtreme_doctors_`git rev-parse HEAD | cut -c1-6`
docker build -t "$username/$image_name" ..

# Return to previous dir
popd > /dev/null

# End
echo
echo "Press enter to exit..."
read _
