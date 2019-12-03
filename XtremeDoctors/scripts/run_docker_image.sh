# Get image name
if [ -n "$1" ]; then
    image_name="xtreme_doctors_$1"
else
    image_name=xtreme_doctors_`git rev-parse HEAD`
fi

# Run container in synchronous mode. To run in detached change "-it" to "-d"
container_name=xtreme_doctors
port=2137
echo "Hosting on http://localhost:$port"
docker run -it --rm -p $port:80 --name "$container_name" "$image_name"

# If failed, print docker images
if [ $? != 1 ]; then
    echo
    echo "docker images"
    docker images
fi
