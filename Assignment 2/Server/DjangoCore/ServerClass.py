from enum import Enum

# Our temporary database for users
# To create user just create an instance with all hard coded values :D
class ServerClass(Enum):
    PONY = (12340, "Pony", 10, 400, 10,
            [(1, 1, 1), (2, 1, 1), (3, 3, 1), (4, 6, 16), (5, 1, 1), (6, 9, 16)])
    TIGER = (5678, "Tiger", 20, 100, 30, [])

    def __init__(self, uid, username, level, gold, armorPoints, listOfCards):
        self._uid = uid
        self._username = username
        self._level = level
        self._gold = gold
        self._armorPoints = armorPoints
        # List of triples (type, lvl, count)
        self._listOfCards = listOfCards

    @property
    def uid(self):
        return self._uid

    @property
    def username(self):
        return self._username

    @property
    def level(self):
        return self._level

    @property
    def gold(self):
        return self._gold

    @property
    def armorPoints(self):
        return self._armorPoints

    @property
    def listOfCards(self):
        return self._listOfCards

    @classmethod
    def get_user_by_name(cls, username):
        for user in cls:
            if user.username.lower() == username.lower():
                return user
        return None

    @classmethod
    def get_user_by_id(cls, uid):
        for user in cls:
            if user.uid == uid:
                return user
        return None

nextpid = 3
class Parties(Enum):
    PARTY1 = (1, [12340, 5678], [])
    PARTY2 = (2, [5678], [])
    # memberCount, hp, shield are calculated in the C# class
    def __init__(self, pid, members, memberPOI):
        self._pid = pid
        self._members = members
        self._memberPOI = memberPOI

    @property
    def pid(self):
        return self._pid

    @property
    def members(self):
        return self._members

    @property
    def memberPOI(self):
        return self._memberPOI

    @classmethod
    def get_party_by_pid(cls, pid):
        for party in cls:
            if party.pid == pid:
                return party
        return None

    @classmethod
    def join_party(cls, pid, uid):
        for party in cls:
            if party.pid == pid:
                if uid in party.members:
                    return "already"
                party._members.append(uid)
                return "Success"
        return "no Party"

    @classmethod
    def leave_party(cls, pid, uid):
        for party in cls:
            if party.pid == pid:
                if uid in party.members:
                    party._members.remove(uid)
                    return "Success"
                return "Already"
        return "no Party"

    @classmethod
    def create_party(cls, uid):
        #TODO Party in Datenbank einfügen
        party = (nextpid, [uid], [])
        nextpid += 1
        return -2




class ServerUtility:
    color = 1
    uid = 1