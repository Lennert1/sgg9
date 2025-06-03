from rest_framework.decorators import api_view
from rest_framework.response import Response

color=1

@api_view(['POST'])
def change_color(request):
    color_data = request.data.get("color")
    color = int(color_data)
    return Response({"status": "received", "color": color_data})