from django.shortcuts import render
from rest_framework.response import Response
import ServerClass

# Create your views here.
def change_color(request):
    color_data = request.data.get("color")
    ServerClass.color = int(color_data)
    print("Empfangene Farbe (REST):", color_data)
    return Response({"status": "received", "color": color_data})