import grpc 
from google.protobuf.timestamp_pb2 import Timestamp
import enums_pb2 as Enums
import MeterReader_pb2 as MeterReader
import MeterReader_pb2_grpc as MeterReaderService
def main():
	print("Calling gRPC Service")
	channel = grpc.insecure_channel("localhost:5001")
	stub = MeterReaderService.MeterReadingServiceStub(channel)

	request = MeterReader.ReadingPacket(successful = Enums.ReadingStatus.SUCCESS)
	reading = MeterReader.ReadingMessage(customerId = 1,
									  readingValue = 1,
									  readingTime = Timestamp().GetCurrentTime())

	request.readings.append(reading)

	result = stub.AddReading(request)
	if(result.success == Enums.ReadingStatus.SUCCESS):
	   print ("Success")
	else:
		print("Failure")
main()

