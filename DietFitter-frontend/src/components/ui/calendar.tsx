import { Datepicker } from "flowbite-react";


export default function Calendar() {
  return (
    <Datepicker
    className="pointer-events-none"
    language="pl-PL"
    labelTodayButton="Dzisiaj"
    labelClearButton="Wyczyść"
  />
  

  );
}
