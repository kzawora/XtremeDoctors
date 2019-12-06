# Run container in synchronous mode. To run in detached change "-it" to "-d"
image_name="xtreme_doctors"
container_name="xtreme_doctors"
public_port=10000
echo "Hosting on http://localhost:$public_port"
docker run -it --rm -p $public_port:9080 --name "$container_name" "$image_name"

# If failed, print docker images
if [ $? != 1 ]; then
    echo
    echo "docker images"
    docker images
fi

# End
echo
echo "Press enter to exit..."
read _
