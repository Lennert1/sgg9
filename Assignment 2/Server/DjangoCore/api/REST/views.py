from bson import ObjectId
from django.http import JsonResponse
from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from rest_framework.response import Response
from ServerClass import ServerClass, Parties, ServerUtility
import json
import utilities
from api.models import *


# Create your views here.
@csrf_exempt
def change_color(request):
    print(request.body)
    color_data = json.loads(request.body).get("color")
    ServerUtility.color = int(color_data)
    print("Empfangene Farbe (REST):", color_data)
    return  utilities.server_message_response(f"color: {color_data}","DATA", status = 200)

@csrf_exempt
def login(request):
    name = json.loads(request.body).get("name")
    uid = json.loads(request.body).get("uid")

    print("Name: ", name)
    print("UID: ", uid)
    return utilities.server_message_response("received","STATUS", status = 200)

@csrf_exempt
def update_color(request):
    msg = json.dumps({
        "color": f"{ServerUtility.color}"
    })
    return utilities.server_message_response(msg,"DATA", status = 200)

# =============================== Stuff that Han did ========================================

# Receives input from Login UI and checks if user exists
# If yes a json is sent to the frontend
@csrf_exempt
def check_login(request):
    if request.method != "POST":
        return utilities.server_message_response("Not a POST request was sent", "405", status = 405)

    # Checking the login based on the username
    try:
        data = json.loads(request.body)
        username = data.get('name')
    except json.JSONDecodeError:
        return utilities.server_message_response("Invalid Json!", "400", status = 400)

    # Class method. Searches all the instances and returns None if user does not exist
    user = ServerClass.get_user_by_name(username)

    if user is None:
        return utilities.server_message_response("User was not found", "404", status=404)
    else:
        user_data = {
            "name": user.username,
            "uid": user.uid,
            "lvl": user.level,
            "gold": user.gold,
            "armorPoints": user.armorPoints,
            "cards": [{"type": card[0], "lvl": card[1], "count": card[2]} for card in user.listOfCards]
        }
        # Sends a Json Response back to the frontend
        return JsonResponse(user_data, status=200)

# This method is not finished yet
# This should add an instance to the ServerClass/ database
@csrf_exempt
def register(request):
    if request.method == "POST":
        try:
            # This is the new user sent by unity
            data = json.loads(request.body)

            name = data.get("name")
            print(data)

            # A call to see if the user already exists
            # If user already exists send status 409 code back
            if User.objects.filter(name=name).exists():
                return utilities.server_message_response("User with that Name already exists!", "409", status = 409)

            # Create user

            user = User.objects.create(
                name = name,
                lvl = data.get("lvl"),
                gold = data.get("gold"),
                upgradePoints = data.get("upgradePoints"),
                selectedCharacter = data.get("selectedCharacter"),
                cards = data.get("cards"),
                characters = data.get("characters"),
                friends = data.get("friends")
            )
            user.save()
            print("Anzahl User in DB:", User.objects.count())

            # TODO: Assign UID that does not exist
            return JsonResponse(user, status=200)
        except Exception as e:
            print(e)
            return utilities.server_message_response("received","STATUS", status=400)
    else:
        return utilities.server_message_response("received","STATUS", status=405)





@csrf_exempt
def updateData(request):
    if request.method == "POST":
        try:
            # This is the userdata received from the front end
            data = json.loads(request.body)
            uid = data.get("uid")
            print(data)
            print(uid)

            # Send the updated data to the database

            return utilities.server_message_response("received", "STATUS", status=200)
        except Exception as e:
            return utilities.server_message_response("received", "STATUS", status=400)
    else:
        return utilities.server_message_response("received", "STATUS", status=405)

# Searches in the database for the id and returns the data in a json
@csrf_exempt
def userById(request):
    #if request.method == "GET":

        data = json.loads(request.body)
        uid = ObjectId(data.get("uid"))
        print(data)
        print(uid)
        # Search for user
        user = User.objects.filter(id=uid).first()
        print(f"user: {user}")
        if user is not None:
            #helper functions in utility
            user_data = utilities.serialize_user(user)
            print(user_data)
            return JsonResponse(user_data)
        else:
            return JsonResponse({'error': 'User not found'}, status=404)
    #else:
    #    return JsonResponse({'error': 'Invalid request method'}, status=400)


@csrf_exempt
def partyById(request, pid):
    if request.method == "POST":
        # Search for user
        party = Parties.get_party_by_pid(pid)
        if party is not None:
            party_data = {
                "pid": party.pid,
                "members": [ member for member in party.members],
                # I dunno where this is even used
                "memberPoIids": []
            }
            return JsonResponse(party_data, status=200)
        else:
            return JsonResponse({'error': 'Party not found'}, status=404)
    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)

#TODO Von Mock Datenback auf echte umschreiben bei ServerClass
@csrf_exempt
def allParties(request):
    if request.method == "POST":
        all_parties_data = []

        # TODO Parties aus der Datenbank pls
        for party in Parties:
            party_data = {
                "pid": party.pid,
                "members": [member for member in party.members],
                "memberPoIids": [poi for poi in party.memberPOI]
            }
            all_parties_data.append(party_data)

        print(all_parties_data)
        return JsonResponse(all_parties_data, safe=False)
    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)


@csrf_exempt
def joinParty(request):
    if request.method == "POST":
        pid, uid = 0, "0"
        try:
            data = json.loads(request.body)
            pid = data.get('pid')
            uid = data.get('uid')
        except json.JSONDecodeError:
            return utilities.server_message_response("Invalid Json!", "400", status=400)
        # TODO Parties aus der Datenbank pls
        result = Parties.join_party(pid, uid)

        if result == "Already":
            return JsonResponse({'error': 'User already in Party'}, status=404)
        elif result == "Success":
            party = Parties.get_party_by_pid(pid)
            party_data = {
                "pid": party.pid,
                "members": [member for member in party.members],
                # I dunno where this is even used
                "memberPoIids": []
            }
            return JsonResponse(party_data, status=200)
        elif result == "no Party":
            return JsonResponse({'error': f"No Party with pid {pid}"}, status=404)
    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)



@csrf_exempt
def leaveParty(request):
    if request.method == "POST":
        pid, uid = 0, "0"
        try:
            data = json.loads(request.body)
            pid = data.get('pid')
            uid = data.get('uid')
        except json.JSONDecodeError:
            return utilities.server_message_response("Invalid Json!", "400", status=400)
        # TODO Parties aus der Datenbank pls
        result = Parties.leave_party(pid, uid)

        if result == "Already":
            return JsonResponse({'error': 'User not in Party'}, status=404)
        elif result == "Success":
            party = Parties.get_party_by_pid(pid)
            party_data = {
                "pid": party.pid,
                "members": [member for member in party.members],
                # I dunno where this is even used
                "memberPoIids": []
            }
            return JsonResponse(party_data, status=200)
        elif result == "no Party":
            return JsonResponse({'error': f"No Party with pid {pid}"}, status=404)
    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)




