from bson import ObjectId
from django.http import JsonResponse
from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from rest_framework.response import Response
from ServerClass import ServerClass, Parties, ServerUtility
import json
import utilities
from api.models import *
from utilities import serialize_party


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
    print("username: ", username)
    user = User.objects.filter(name=username).first()

    if user is None:
        return utilities.server_message_response("User was not found", "404", status=404)
    else:
        print("User: ", username)
        user_data = utilities.serialize_user(user)
        print(user_data)
        return JsonResponse(user_data)

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

            cards = [Card(
                type=card["type"],
                lvl=card["lvl"],
                count=card["count"]
            ) for card in data.get("cards")]

            characters = [Character(
                type=character["type"],
                lvl=character["lvl"],
                hp=character["hp"],
                deck=[Card(
                    type=deck_card["type"],
                    lvl=deck_card["lvl"],
                    count=deck_card["count"]
                ) for deck_card in character["deck"]]
            ) for character in data.get("characters")]

            user = User.objects.create(
                name=name,
                lvl=1,
                gold=0,
                upgradePoints=0,
                selectedCharacter=0,
                cards=cards,
                characters=characters,
                friends=data.get("friendsUID")
            )
            print("hier")
            user.save()
            print("Anzahl User in DB:", User.objects.count())

            print(user.id)

            user_data = utilities.serialize_user(user)
            print(user_data)
            return JsonResponse(user_data)

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

            # update user entries
            cards = [Card(
                type=card["type"],
                lvl=card["lvl"],
                count=card["count"]
            ) for card in data.get("cards")]

            characters = [Character(
                type=character["type"],
                lvl=character["lvl"],
                hp=character["hp"],
                deck=[Card(
                    type=deck_card["type"],
                    lvl=deck_card["lvl"],
                    count=deck_card["count"]
                ) for deck_card in character["deck"]]
            ) for character in data.get("characters")]

            user = User.objects.filter(id=ObjectId(uid)).first()
            user.name = data.get("name")
            user.lvl = int(data.get("lvl"))
            user.gold = int(data.get("gold"))
            user.upgradePoints = int(data.get("upgradePoints"))
            user.selectedCharacter = int(data.get("selectedCharacter"))
            user.cards = cards
            user.characters = characters
            user.friends = data.get("friendsUID")
            user.save()

            user_data = utilities.serialize_user(user)
            return JsonResponse(user_data)
        except Exception as e:
            print(e)
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
            return utilities.server_message_response("no user with that uid", "ERROR", status=404)
    #else:
    #    return JsonResponse({'error': 'Invalid request method'}, status=400)


@csrf_exempt
def partyById(request):
    if request.method == "POST":
        data = json.loads(request.body)
        pid = ObjectId(data.get("pid"))
        # Search for party
        party = Party.objects.filter(id=ObjectId(pid)).first()
        if party is not None:
            #helper function in utility
            party_data = utilities.serialize_party(party)
            return JsonResponse(party_data, status=200)
        else:
            return JsonResponse({'error': 'Party not found'}, status=404)
    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)


@csrf_exempt
def allParties(request):
    if request.method == "POST":
        all_parties_data = []

        all_parties = Party.objects.filter()
        for party in all_parties:
            party_data = serialize_party(party)
            all_parties_data.append(party_data)

        print(all_parties_data)
        return JsonResponse(all_parties_data, safe=False)
    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)


@csrf_exempt
def joinParty(request):
    if request.method == "POST":
        try:
            data = json.loads(request.body)
            pid = data.get('pid')
            uid = ObjectId(data.get('uid'))
        except json.JSONDecodeError:
            return utilities.server_message_response("Invalid Json!", "400", status=400)

        party = Party.objects.filter(id=ObjectId(pid)).first()
        if party is not None:
            members = party.members
            members.append(uid)
            party.members = members
            party.save()
            party_data = utilities.serialize_party(party)
            return JsonResponse(party_data, status=200)
        else:
            return utilities.server_message_response("Party not found", "ERROR", status=404)

    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)



@csrf_exempt
def leaveParty(request):
    if request.method == "POST":
        try:
            data = json.loads(request.body)
            pid = data.get('pid')
            uid = ObjectId(data.get('uid'))
        except json.JSONDecodeError:
            return utilities.server_message_response("Invalid Json!", "400", status=400)

        party = Party.objects.filter(id=ObjectId(pid)).first()
        if party is not None:
            members = party.members
            members.remove(uid)
            party.members = members
            party.save()

        return utilities.server_message_response("leaveParty successful", "STATUS", status=200)
    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)



@csrf_exempt
def createParty(request):
    if request.method == "POST":
        try:
            data = json.loads(request.body)
            uid = ObjectId(data.get('uid'))
            PoIID = data.get('PoIID')
        except json.JSONDecodeError:
            return utilities.server_message_response("Invalid Json!", "400", status=400)

        party = Party(
            hp = 0,
            shield = 0,
            members=[uid],
            PoIIDs = [PoIID]
        )
        party.save()
        party_data = utilities.serialize_party(party)
        return JsonResponse(party_data, status=200)
    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)


####################################-Battle Arena-########################################
@csrf_exempt
def getBattleArena(request):
    try:
        data = json.loads(request.body)
        pid = ObjectId(data.get('pid'))
    except json.JSONDecodeError:
        return utilities.server_message_response("Invalid Json!", "400", status=400)

    battleArena = BattleArena.objects.get(pid=ObjectId(pid))
    ba_data = utilities.serialize_battle_arena(battleArena)
    return JsonResponse(ba_data, status=200)


@csrf_exempt
def updateBattleArena(request):
    try:
        data = json.loads(request.body)
        pid = ObjectId(data.get('pid'))
    except json.JSONDecodeError:
        return utilities.server_message_response("Invalid Json!", "400", status=400)

    battleArena = BattleArena.objects.get(pid=ObjectId(pid))




@csrf_exempt
def createBattleArena(request):

    try:
        data = json.loads(request.body)
    except json.JSONDecodeError:
        return utilities.server_message_response("Invalid Json!", "400", status=400)

    en = data.get("enemy")
    enemy = Enemy(
        type = en["type"],
        lvl = en["level"],
        hp = en["hp"],
        deck = [Card(
            type=deck_card["type"],
            lvl=deck_card["lvl"],
            count=deck_card["count"]
        ) for deck_card in en["deck"]]
    )

    rewardCards = [Card(
        type = card["type"],
        lvl = card["level"],
        count = card["count"]
    )for card in data.get("rewardCards")]

    playerCards = [Card(
        type=card["type"],
        lvl=card["level"],
        count=card["count"]
    )for card in data.get("playerCards")]

    enemyCards = [Card(
        type=card["type"],
        lvl=card["level"],
        count=card["count"]
    ) for card in data.get("enemyCards")]

    battleArena = BattleArena.objects.create(
        pid = ObjectId(data.get('pid')),
        enemy = enemy,
        rewardGold = data.get("rewardGold"),
        rewardCards = rewardCards,
        rewardUpgradePoints = data.get("rewardUpgradePoints"),
        teamHP = data.get("teamHP"),
        teamShield = data.get("teamShield"),
        playerChecks = data.get("playerChecks"),
        playerCards = playerCards,
        enemyCards = enemyCards,
        battleState = data.get("battleState")
    )
    battleArena.save()

    ba_data = utilities.serialize_battle_arena(battleArena)
    return JsonResponse(ba_data, status=200)
